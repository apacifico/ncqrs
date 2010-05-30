﻿using System;
using System.Collections.Generic;
using Ncqrs.Domain;
using Ncqrs.Eventing;
using Sample.Events;
using System.Diagnostics.Contracts;

namespace Sample.Domain
{
    public class MessageMappedWithExpressions : AggregateRootMappedWithExpressions
    {
        private String _messageText;
        private DateTime _creationDate;

        public MessageMappedWithExpressions(Guid messageId, String text)
        {
            var e = new NewMessageAdded
            {
                MessageId = messageId,
                Text = text,
                CreationDate = DateTime.UtcNow
            };

            ApplyEvent(e);
        }

        public void UpdateMessageText(String newMessageText)
        {
            var e = new MessageTextUpdated
            {
                MessageId = Id,
                UpdatedMessageText = newMessageText,
                ChangeDate = DateTime.UtcNow
            };

            ApplyEvent(e);
        }

        public void TestMethod<TParam>(Action<TParam> action, TParam param)
        {
            action(param);
        }

        public override void InitMappingRules()
        {
            TestMethod(UpdateMessageText, "test");
        }   

        private void OnNewMessageAdded(NewMessageAdded e)
        {
             Id = e.MessageId;
            _messageText = e.Text;
            _creationDate = e.CreationDate;
        }

        private void OnMessageTextUpdated(MessageTextUpdated e)
        {
            Contract.Assert(e.MessageId == Id, "The MessageId should be the same as "+
                                               "the Id of this instance. Since otherwise, "+
                                               "the event is not owned by this instance and "+
                                               "should never reach this point.");

            _messageText = e.UpdatedMessageText;
        }
    }
}