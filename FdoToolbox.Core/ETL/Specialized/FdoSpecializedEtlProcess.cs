using System;
using System.Collections.Generic;
using System.Text;

namespace FdoToolbox.Core.ETL.Specialized
{
    /// <summary>
    /// A specialized form of ETL process.
    /// </summary>
    public abstract class FdoSpecializedEtlProcess : EtlProcess, IFdoSpecializedEtlProcess
    {
        /// <summary>
        /// Fires the feature processed.
        /// </summary>
        /// <param name="count">The count.</param>
        protected void FireFeatureProcessed(int count)
        {
            FeatureProcessed(this, new FeatureCountEventArgs(count));
        }

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        protected void SendMessage(string msg)
        {
            ProcessMessage(this, new MessageEventArgs(msg));
        }

        /// <summary>
        /// Sends the message formatted.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        protected void SendMessageFormatted(string format, params object[] args)
        {
            ProcessMessage(this, new MessageEventArgs(string.Format(format, args)));
        }

        /// <summary>
        /// Fires when a feature has been processed
        /// </summary>
        public event FeatureCountEventHandler FeatureProcessed = delegate { };

        /// <summary>
        /// Fires when a message is sent from this process
        /// </summary>
        public event MessageEventHandler ProcessMessage = delegate { };

        /// <summary>
        /// Called when a row is processed.
        /// </summary>
        /// <param name="op">The operation.</param>
        /// <param name="dictionary">The dictionary.</param>
        protected override void OnFeatureProcessed(FdoToolbox.Core.ETL.Operations.FdoOperationBase op, FdoRow dictionary)
        {
            //We want to avoid log chatter on specialized ETL processes so suppress the base call   
        }

        /// <summary>
        /// Called when this process has finished processing.
        /// </summary>
        /// <param name="op">The op.</param>
        protected override void OnFinishedProcessing(FdoToolbox.Core.ETL.Operations.FdoOperationBase op)
        {
            //We want to avoid log chatter on specialized ETL processes so suppress the base call
        }

        /// <summary>
        /// Casts this to a <see cref="EtlProcess"/>
        /// </summary>
        /// <returns></returns>
        public EtlProcess ToEtlProcess()
        {
            return this;
        }

        /// <summary>
        /// Called when the process has completed
        /// </summary>
        protected override void OnProcessCompleted()
        {
            ProcessCompleted(this, EventArgs.Empty);
        }

        /// <summary>
        /// Occurs when [process completed].
        /// </summary>
        public event EventHandler ProcessCompleted = delegate { };

        /// <summary>
        /// Occurs when [process aborted].
        /// </summary>
        public event EventHandler ProcessAborted = delegate { };
    }
}