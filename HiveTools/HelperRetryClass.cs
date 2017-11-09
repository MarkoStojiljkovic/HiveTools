using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HiveTools
{
    class HelperRetryClass
    {
        private static int MAX_ATTEMPS = 3;

        public static T Do<T>(Func<T> action, TimeSpan retryInterval)
        {
            //var exceptions = new List<Exception>();

            for (int attempted = 0; attempted < MAX_ATTEMPS; attempted++)
            {
                try
                {
                    if (attempted > 0)
                    {
                        Thread.Sleep(retryInterval);
                    }
                    FormCustomConsole.WriteLineWithConsole("Do<T>, attempted: " + attempted);
                    return action();
                }
                catch (Exception ex)
                {
                    //exceptions.Add(ex);
                    //Console.WriteLine("Exception: " + ex.Message);
                    FormCustomConsole.WriteLineWithConsole("Exception in Do<T>: " + ex.Message);
                }
            }
            //throw new AggregateException(exceptions);
            throw new Exception("Action not executed successfully " + MAX_ATTEMPS + " times");
        }
        

        /// <summary>
        /// Generic Retry
        /// </summary>
        /// <typeparam name="TResult">return type</typeparam>
        /// <param name="action">Method needs to be executed</param>
        /// <param name="retryInterval">Retry interval</param>
        /// <param name="retryCount">Retry Count</param>
        /// <param name="expectedResult">Expected Result</param>
        /// <param name="isExpectedResultEqual">true/false to check equal 
        /// or not equal return value</param>
        /// <param name="isSuppressException">
        /// Suppress exception is true / false</param>
        /// <returns></returns>
        public static TResult Execute<TResult>(
          Func<TResult> action,
          TimeSpan retryInterval,
          int retryCount,
          TResult expectedResult,
          bool isExpectedResultEqual = true,
          bool isSuppressException = true
           )
        {
            TResult result = default(TResult);

            bool succeeded = false;
            var exceptions = new List<Exception>();

            for (int retry = 0; retry < retryCount; retry++)
            {
                try
                {
                    if (retry > 0)
                        Thread.Sleep(retryInterval);
                    // Execute method
                    result = action();

                    if (isExpectedResultEqual)
                        succeeded = result.Equals(expectedResult);
                    else
                        succeeded = !result.Equals(expectedResult);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }

                if (succeeded)
                    return result;
            }

            if (!isSuppressException)
                throw new AggregateException(exceptions);
            else
                return result;
        }


        public async static Task DoWithRetryAsync(Func<Task> action, TimeSpan sleepPeriod, int tryCount = 3)
        {
            if (tryCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(tryCount));

            while (true)
            {
                try
                {
#warning ASYNC METHOD
                    await action();
                    return; // success!
                }
                catch
                {
                    Console.WriteLine("Retries remaining: " + (tryCount - 1));
                    if (--tryCount == 0)
                        throw;
                    await Task.Delay(sleepPeriod);
                }
            }
        }


    }
    
}
