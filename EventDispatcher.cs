using System.Collections.Generic;
using System;

namespace MyNamespace.Utils
{
    public delegate void EventHandler(short type);
    public delegate void EventHandler<T>(short type,T msg);

    public class EventDispatcher
    {
        private readonly Dictionary<short, List<Delegate>> _eventHandlerDic = new Dictionary<short, List<Delegate>>();
        private readonly Dictionary<short, List<Delegate>> _oneShotHandlerDic = new Dictionary<short, List<Delegate>>();

        private readonly List<List<Delegate>> _tempHandlerList = new List<List<Delegate>>();

        public void ClearEventHandler()
        {
            Dictionary<short, List<Delegate>>.Enumerator etor = _eventHandlerDic.GetEnumerator();
            while (etor.MoveNext())
            {
                etor.Current.Value.Clear();
            }

            etor = _oneShotHandlerDic.GetEnumerator();
            while (etor.MoveNext())
            {
                etor.Current.Value.Clear();
            }

            _tempHandlerList.Clear();
        }

        public void PresizeHandler(short type, int size)
        {
            if (!_eventHandlerDic.ContainsKey(type))
            {
                _eventHandlerDic.Add(type, new List<Delegate>(size));
            }
        }

        public void ClearNullEventHandler()
        {
            int count = _tempHandlerList.Count;
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    List<Delegate> eventListeners = _tempHandlerList[i];
                    int len = eventListeners.Count;
                    int newLen = 0;
                    for (int j = 0; j < len; j++)
                    {
                        if (eventListeners[j] != null)
                        {
                            if (newLen != j)
                                eventListeners[newLen] = eventListeners[j];
                            newLen++;
                        }
                    }
                    eventListeners.RemoveRange(newLen, len - newLen);
                }
                _tempHandlerList.Clear();
            }
        }

        #region 无参数事件

        public void AddEventHandler(short type, EventHandler handler)
        {
            AddEventHandlerInternal(_eventHandlerDic, type, handler);
        }

        public void RemoveEventHandler(short type, EventHandler handler)
        {
            RemoveEventHandlerInternal(_eventHandlerDic, type, handler);
        }

        public void AddOneShotEventHandler(short type, EventHandler handler)
        {
            AddEventHandlerInternal(_oneShotHandlerDic, type, handler);
        }

        public void RemoveOneShotEventHandler(short type, EventHandler handler)
        {
            RemoveEventHandlerInternal(_oneShotHandlerDic, type, handler);
        }

        public void SendEvent(short type)
        {
            List<Delegate> eventListeners = GetEventHandlerList(_eventHandlerDic, type, false);
            if (eventListeners != null)
            {
                int len = eventListeners.Count;
                for (int i = 0; i < len; i++)
                {
                    if (eventListeners[i] == null)
                        continue;
                    EventHandler tEvent = (EventHandler)eventListeners[i];
                    tEvent(type);

                }
            }
            List<Delegate> oneShotListeners = GetEventHandlerList(_oneShotHandlerDic, type, false);
            if (oneShotListeners != null)
            {
                int len = oneShotListeners.Count;
                for (int i = 0; i < len; i++)
                {
                    if (oneShotListeners[i] == null)
                        continue;
                    EventHandler tEvent = (EventHandler)oneShotListeners[i];
                    tEvent(type);
                }
                oneShotListeners.Clear();
            }
        }

        #endregion

        #region 单参数事件

        public void AddEventHandler<T>(short type, EventHandler<T> handler)
        {
            try
            {
                AddEventHandlerInternal(_eventHandlerDic, type, handler);
            }
            catch
            {
                Debug.LogError("trying to add handler error,type:" + type + " handler:" + handler);
            }
        }

        public void RemoveEventHandler<T>(short type, EventHandler<T> handler)
        {
            RemoveEventHandlerInternal(_eventHandlerDic, type, handler);
        }

        public void AddOneShotEventHandler<T>(short type, EventHandler<T> handler)
        {
            AddEventHandlerInternal(_oneShotHandlerDic, type, handler);
        }

        public void RemoveOneShotEventHandler<T>(short type, EventHandler<T> handler)
        {
            RemoveEventHandlerInternal(_oneShotHandlerDic, type, handler);
        }

        public void SendEvent<T>(short type, T msg)
        {
            List<Delegate> eventListeners = GetEventHandlerList(_eventHandlerDic, type, false);
            if (eventListeners != null)
            {
                int len = eventListeners.Count;
                for (int i = 0; i < len; i++)
                {
                    if (eventListeners[i] == null)
                        continue;
                    EventHandler<T> tEvent = (EventHandler<T>)eventListeners[i];


                    tEvent(type, msg);
                }
            }

            List<Delegate> oneShotListeners = GetEventHandlerList(_oneShotHandlerDic, type, false);
            if (oneShotListeners != null)
            {
                int len = oneShotListeners.Count;
                for (int i = 0; i < len; i++)
                {
                    if (oneShotListeners[i] == null)
                        continue;
                    EventHandler<T> tEvent = (EventHandler<T>)oneShotListeners[i];

                    tEvent(type, msg);

                }

                oneShotListeners.Clear();
            }
        }

        #endregion

        #region Private

        private void AddEventHandlerInternal(Dictionary<short, List<Delegate>> dic, short type, Delegate handler)
        {
            List<Delegate> eventListeners = GetEventHandlerList(dic, type, true);
            bool newEvent = true;

#if DEBUG
            if (eventListeners.Contains(handler))
            {
                Debug.LogError("repeat to add " + handler);
                newEvent = false;
            }
#endif

            if (newEvent)
            {
                eventListeners.Add(handler);
            }
        }

        private void RemoveEventHandlerInternal(Dictionary<short, List<Delegate>> dic, short type, Delegate handler)
        {
            List<Delegate> eventListeners = GetEventHandlerList(dic, type, false);
            if (eventListeners == null)
                return;

            int index = eventListeners.IndexOf(handler);
            if (index != -1)
            {
                eventListeners[index] = null;
                if (!_tempHandlerList.Contains(eventListeners))
                    _tempHandlerList.Add(eventListeners);
            }
        }

        private List<Delegate> GetEventHandlerList(Dictionary<short, List<Delegate>> dic, short type, bool autoCreate)
        {
            List<Delegate> eventList;
            if (dic.TryGetValue(type, out eventList))
                return eventList;
            else
            {
                if (autoCreate)
                {
                    eventList = new List<Delegate>();
                    dic.Add(type, eventList);
                    return eventList;
                }
                return null;
            }
        }

        #endregion
    }
}