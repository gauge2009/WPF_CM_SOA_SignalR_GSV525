using System;
using System.Linq;
using System.Activities;
using System.Activities.Statements;
using System.Threading;
using System.Collections.Generic;
using Alayaz.WF.FlowchartNumberGuessWorkflow;
using Alayaz.WF.StateMachineNumberGuessWorkflow;

namespace Alayaz.WF.NumberGuessWorkflowHost
{

    class Program
    {
        /// <summary>
        /// 此代码将创建一个 WorkflowApplication，订阅三个工作流生命周期事件，通过调用 Run 来启动工作流，然后等待工作流完成。 工作流完成时，将设置 AutoResetEvent，并且宿主应用程序也完成。
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //Activity workflow1 = new KeyActivity();
            //WorkflowInvoker.Invoke(workflow1);

            AutoResetEvent syncEvent = new AutoResetEvent(false);
            AutoResetEvent idleEvent = new AutoResetEvent(false);

            var inputs = new Dictionary<string, object>() { { "MaxNumber", 100 } };

            WorkflowApplication wfApp =
                  //new WorkflowApplication(new KeyActivity(), inputs);
                  //new WorkflowApplication(new FlowActivity(), inputs);
                  new WorkflowApplication(new StateMachineActivity(), inputs);


            wfApp.Completed = delegate (WorkflowApplicationCompletedEventArgs e)
            {
                int Turns = Convert.ToInt32(e.Outputs["Turns"]);
                Console.WriteLine("祝贺！ 你用了 {0} 次得以猜中.", Turns);

                syncEvent.Set();
            };


            wfApp.Aborted = delegate (WorkflowApplicationAbortedEventArgs e)
            {
                Console.WriteLine(e.Reason);
                syncEvent.Set();
            };

            wfApp.OnUnhandledException = delegate (WorkflowApplicationUnhandledExceptionEventArgs e)
            {
                Console.WriteLine(e.UnhandledException.ToString());
                return UnhandledExceptionAction.Terminate;
            };

            //每次工作流变为空闲状态等待下一个猜测时，都会调用此处理程序并设置 idleAction AutoResetEvent。 
            wfApp.Idle = delegate (WorkflowApplicationIdleEventArgs e)
            {
                idleEvent.Set();
            };

            wfApp.Run();


            //syncEvent.WaitOne();

            //下面步骤中的代码使用 idleEvent 和 syncEvent 来确定工作流是在等待下一个猜测还是已完成。 
            // 在本示例中，宿主应用程序在 Completed 和 Idle 处理程序中使用自动重置事件将宿主应用程序与工作流的进度同步。 在继续执行书签之前，不需要阻止并等待工作流变为空闲状态，但在此示例中需要同步事件，以使宿主知道工作流是否已完成，或是否在等待使用 Bookmark 的更多用户输入。
            // Loop until the workflow completes.
            WaitHandle[] handles = new WaitHandle[] { syncEvent, idleEvent };
            while (WaitHandle.WaitAny(handles) != 0)
            {
                // Gather the user input and resume the bookmark.
                bool validEntry = false;
                while (!validEntry)
                {
                    int Guess;
                    if (!Int32.TryParse(Console.ReadLine(), out Guess))
                    {
                        Console.WriteLine("只能猜整数.");
                    }
                    else
                    {
                        validEntry = true;
                        //此句保证工作流迭代继续，相当于continue / goto EnterGuess
                        wfApp.ResumeBookmark("EnterGuess", Guess);
                    }
                }
            }



        }
    }
}
