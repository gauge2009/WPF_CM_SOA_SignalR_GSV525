using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Alayaz.CM.DN432.WebCrawl.ViewModels;
using Alayaz.CM.DN432.WebCrawl.Views;

namespace Alayaz.CM.DN432.WebCrawl.Coroutines
{
    public class Loader : IResult
    {
        readonly string message;
        readonly bool hide;

        public Loader(string message)
        {
            this.message = message;
        }

        public Loader(bool hide)
        {
            this.hide = hide;
        }

        public void Execute(CoroutineExecutionContext context)
        {
 
            var view = context.View as FrameworkElement;
            while (view != null)
            {
                /* var busyIndicator = view as BusyIndicator;
                 if (busyIndicator != null)
                 {
                     if (!string.IsNullOrEmpty(message))
                         busyIndicator.BusyContent = message;
                     busyIndicator.IsBusy = !hide;
                     break;
                 }*/
               var  vw = view as StartScreenView;
                if (vw != null)
                {
                    TextBlock tips = vw.FindName("Tips") as TextBlock;
                    if (tips != null)
                    {
                        tips.Text = !string.IsNullOrEmpty(message)?message:"";

                    }
                   
                     break;
                }

                view = view.Parent as FrameworkElement;
            }

            var vm = context.Target as StartScreenViewModel;
            while (vm != null)
            {
                vm.IsBusy = !hide;
                 vm.BusyText = !string.IsNullOrEmpty(message)? message:"";
                 
              
                break;
            }

            Completed(this, new ResultCompletionEventArgs());
        }

        public event EventHandler<ResultCompletionEventArgs> Completed = delegate { };

        public static IResult Show(string message = null)
        {
            return new Loader(message);
        }

        public static IResult Hide()
        {
            return new Loader(true);
        }
    }

}
