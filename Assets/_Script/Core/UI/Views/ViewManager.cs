using Core.Singleton;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.UI
{
    public class ViewManager : Singleton<ViewManager>
    {
        List<View> m_Views;

        View m_CurrentView;

        readonly Stack<View> m_History = new();

        protected override void Awake()
        {
            base.Awake();
            m_Views = GetComponentsInChildren<View>(true).ToList();
            Init();
        }

        void Init()
        {
            foreach (var view in m_Views)
                view.Hide();
            m_History.Clear();
        }
        public T GetView<T>() where T : View
        {
            foreach (var view in m_Views)
            {
                if (view is T tView)
                {
                    return tView;
                }
            }

            return null;
        }
        public void Show<T>(bool keepInHistory = true) where T : View
        {
            foreach (var view in m_Views)
            {
                if (view is T)
                {
                    Show(view, keepInHistory);
                    break;
                }
            }
        }

        public void Show(View view, bool keepInHistory = true)
        {
            view.Show();
            m_CurrentView = view;
        }
        public void GoBack()
        {
            if (m_History.Count != 0)
            {
                Show(m_History.Pop(), false);
            }
        }
    }
}