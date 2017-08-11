using System;
using System.Collections.Generic;
using VkNet;
using VkNet.Enums.Filters;

namespace SuicideAlpha
{
    class Messages
    {
        private VkApi app;
        Dictionary<long?, string> IDDictionary;

        public Messages(VkApi app)
        {
            this.app = app;
            GetIDDictonary();
        }
        //для быстрого доступа, чтобы показать кто писал сообщение
        private void GetIDDictonary()
        {
            var listFriends = app.Friends.Get(new VkNet.Model.RequestParams.FriendsGetParams
            {
                UserId = app.UserId,
                Fields = ProfileFields.FirstName | ProfileFields.LastName,
            });

            IDDictionary = new Dictionary<long?, string>();

            foreach (var friend in listFriends)
                IDDictionary[friend.Id] = friend.FirstName + " " + friend.LastName;

            //добавим себя в список друзей
            IDDictionary.Add(app.UserId.Value, GetNameByID(app, app.UserId.Value));
        }
        //получить сообщения за сутки
        public string GetMessages()
        {
            string resultMessage = String.Empty;
            var listMessages = app.Messages.Get(new VkNet.Model.RequestParams.MessagesGetParams
            {
                Count = 200,
                Out = VkNet.Enums.MessageType.Received,
                TimeOffset = 86400,//в секундах, 86400 == сутки
                Filters = 0,
                PreviewLength = 0,
            });

            //Список ID собеседников
            List<long?> listIDInterlocutor = new List<long?>();

            foreach (var message in listMessages.Messages)
            {
                if (!listIDInterlocutor.Contains(message.UserId))
                    listIDInterlocutor.Add(message.UserId);
            }

            foreach (var id in listIDInterlocutor)
            {
                resultMessage += (ReturnDialogText(app, id, IDDictionary)) + "\n";
                resultMessage += "-----------------------------------\n";
            }

            return resultMessage;
        }

        private string ReturnDialogTextForAnalys(long? userID)
        {
            string str = String.Empty;

            var Dialog = app.Messages.GetHistory(new VkNet.Model.RequestParams.MessagesGetHistoryParams
            {
                Count = 100,
                Offset = 0,
                Reversed = false,
                UserId = userID
            });

            foreach (var m in Dialog.Messages)
                str += m.Body + "\n";

            return str;
        }

        public string GetMessagesForAnalys()
        {
            string resultMessage = String.Empty;
            var listMessages = app.Messages.Get(new VkNet.Model.RequestParams.MessagesGetParams
            {
                Count = 200,
                Out = VkNet.Enums.MessageType.Received,
                TimeOffset = 86400,//в секундах, 86400 == сутки
                Filters = 0,
                PreviewLength = 0,
            });

            //Список ID собеседников
            List<long?> listIDInterlocutor = new List<long?>();

            foreach (var message in listMessages.Messages)
            {
                if (!listIDInterlocutor.Contains(message.UserId))
                    listIDInterlocutor.Add(message.UserId);
            }

            foreach (var id in listIDInterlocutor)
            {
                resultMessage += ReturnDialogTextForAnalys(id) + "\n";
                resultMessage += "-----------------------------------\n";
            }

            return resultMessage;

        }

        //Возвращаем текст диалог с собеседником
        private string ReturnDialogText(VkApi app, long? userID, Dictionary<long?, string> dicID)
        {
            string str = String.Empty;

            var Dialog = app.Messages.GetHistory(new VkNet.Model.RequestParams.MessagesGetHistoryParams
            {
                Count = 100,
                Offset = 0,
                Reversed = false,
                UserId = userID
            });

            foreach (var m in Dialog.Messages)
            {
                if (dicID.ContainsKey(m.FromId))
                {
                    str += "[" + dicID[m.FromId] + "]" + "\n";
                    str += m.Body + "\n";
                }
                else
                {
                    str += "[" + GetNameByID(app, m.FromId) + "]" + "\n";
                    str += m.Body + "\n";
                }
            }
            return str;
        }
        //очень затратная операция
        private string GetNameByID(VkApi app, long? id)
        {
            var person = app.Users.Get((long)id, ProfileFields.FirstName | ProfileFields.LastName);
            return person.FirstName + " " + person.LastName;
        }
    }
}
