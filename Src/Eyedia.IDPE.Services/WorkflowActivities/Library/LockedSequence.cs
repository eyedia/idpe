#region Copyright Notice
/* Copyright (c) 2017, Deb'jyoti Das - debjyoti@debjyoti.com
 All rights reserved.
 Redistribution and use in source and binary forms, with or without
 modification, are not permitted.Neither the name of the 
 'Deb'jyoti Das' nor the names of its contributors may be used 
 to endorse or promote products derived from this software without 
 specific prior written permission.
 THIS SOFTWARE IS PROVIDED BY Deb'jyoti Das 'AS IS' AND ANY
 EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 DISCLAIMED. IN NO EVENT SHALL Debjyoti OR Deb'jyoti OR Debojyoti Das OR Eyedia BE LIABLE FOR ANY
 DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

#region Developer Information
/*
Author  - Deb'jyoti Das
Created - 3/19/2013 11:14:16 AM
Description  - 
Modified By - 
Description  - 
*/
#endregion Developer Information

#endregion Copyright Notice


using System.Activities;
using System.Activities.Statements;
using System.Collections.ObjectModel;
using System.ComponentModel;


namespace Eyedia.IDPE.Services
{
    [Designer("System.Activities.Core.Presentation.SequenceDesigner, System.Activities.Core.Presentation")]
    public sealed class LockedSequence : NativeActivity
    {
        Collection<Activity> children;
        Collection<Variable> variables;
        Variable<int> currentIndex;
        CompletionCallback onChildComplete;

        public InArgument<Job> Job { get; set; }
        public InArgument<WorkerData> Data { get; set; }

        public LockedSequence()
            : base()
        {
            this.children = new Collection<Activity>();
            this.variables = new Collection<Variable>();
            this.currentIndex = new Variable<int>();
        }

        [Browsable(false)]
        public Collection<Activity> Activities
        {
            get
            {
                return this.children;
            }
        }

        [Browsable(false)]
        public Collection<Variable> Variables
        {
            get
            {
                return this.variables;
            }
        }

        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
            //call base.CacheMetadata to add the Activities and Variables to this activity's metadata
            base.CacheMetadata(metadata);
            //add the private implementation variable: currentIndex 
            metadata.AddImplementationVariable(this.currentIndex);
        }

        protected override void Execute(NativeActivityContext context)
        {
            Job job = context.GetValue(this.Job);
            if (job == null)
            {
                WorkerData data = context.GetValue(this.Data);
                data.ThrowErrorIfNull(this.DisplayName);
                job = data.Job;
            }
            lock (job._lock)
            {
                InternalExecute(context, null);
            }
            
        }

        void InternalExecute(NativeActivityContext context, ActivityInstance instance)
        {
            //grab the index of the current Activity
            int currentActivityIndex = this.currentIndex.Get(context);
            if (currentActivityIndex == Activities.Count)
            {
                //if the currentActivityIndex is equal to the count of MySequence's Activities
                //LockedSequence is complete
                return;
            }

            if (this.onChildComplete == null)
            {
                //on completion of the current child, have the runtime call back on this method
                this.onChildComplete = new CompletionCallback(InternalExecute);
            }

            //grab the next Activity in MySequence.Activities and schedule it
            Activity nextChild = Activities[currentActivityIndex];
            context.ScheduleActivity(nextChild, this.onChildComplete);

            //increment the currentIndex
            this.currentIndex.Set(context, ++currentActivityIndex);
        }
    }
}



