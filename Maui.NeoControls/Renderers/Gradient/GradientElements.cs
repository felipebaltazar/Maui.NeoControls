using Maui.NeoControls.Abstractions;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Maui.NeoControls
{
    public class GradientElements<TElement> : ObservableCollection<TElement>
    where TElement : BindableObject, IWithParentElement, IWithCanvasElement
    {
        #region Properties

        public BindableObject? Parent { get; private set; }

        #endregion

        #region Constructors

        public GradientElements() { }

        public GradientElements(IEnumerable<TElement> collection) : base(collection) { }

        public GradientElements(List<TElement> list) : base(list) { }

        #endregion

        #region Internal Methods

        internal void AttachTo(BindableObject parent)
        {
            Parent = parent;

            foreach (var item in Items)
            {
                BindableObject.SetInheritedBindingContext(item, Parent?.BindingContext);
                item.Parent = parent;
            }

            InvalidateParentCanvas();
        }

        internal void Release()
        {
            Parent = null;

            foreach (var item in Items)
            {
                BindableObject.SetInheritedBindingContext(item, null);
                item.Parent = null;
            }
        }

        internal void SetInheritedBindingContext(object bindingContext)
        {
            foreach (var item in Items)
            {
                BindableObject.SetInheritedBindingContext(item, bindingContext);
            }
        }

        #endregion

        #region Protected Methods

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);

            if (Parent == null)
                return;

            var isChanged = false;

            if (e.OldItems != null)
            {
                SetupItems(e.OldItems, null);
                isChanged = true;
            }

            if (e.NewItems != null)
            {
                SetupItems(e.NewItems, Parent);
                isChanged = true;
            }

            if (isChanged)
            {
                InvalidateParentCanvas();

            }
        }

        #endregion

        #region Private Methods

        private void SetupItems(IList items, BindableObject? parent)
        {
            foreach (BindableObject item in items)
            {
                BindableObject.SetInheritedBindingContext(item, parent?.BindingContext);
                var element = (IWithParentElement)item;
                element.Parent = parent;
            }
        }

        private void InvalidateParentCanvas()
        {
            if (Parent is IWithCanvasElement parentWithCanvas)
                parentWithCanvas.InvalidateCanvas();
        }

        #endregion
    }
}
