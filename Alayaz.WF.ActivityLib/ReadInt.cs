using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;

namespace Alayaz.WF.ActivityLib
{

    public sealed class ReadInt : NativeActivity<int> // 如果活动提供单个结果，可以使用 CodeActivity<TResult>,但CodeActivity<TResult> 不支持使用书签，因此使用 NativeActivity<TResult>。
    {
        [RequiredArgument]
        public InArgument<string> BookmarkName { get; set; }

        protected override void Execute(NativeActivityContext context)
        {
            string name = BookmarkName.Get(context);

            if (name == string.Empty)
            {
                throw new ArgumentException("BookmarkName cannot be an Empty string.",
                    "BookmarkName");
            }

            context.CreateBookmark(name, new BookmarkCallback(OnReadComplete));
        }

        // NativeActivity derived activities that do asynchronous operations by calling 
        // one of the CreateBookmark overloads defined on System.Activities.NativeActivityContext 
        // must override the CanInduceIdle property and return true.
        protected override bool CanInduceIdle
        {
            get { return true; }
        }

        void OnReadComplete(NativeActivityContext context, Bookmark bookmark, object state)
        {
            this.Result.Set(context, Convert.ToInt32(state));
        }
    }
}
