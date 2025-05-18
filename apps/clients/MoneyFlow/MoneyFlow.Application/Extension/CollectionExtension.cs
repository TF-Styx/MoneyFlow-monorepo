using System.Collections.ObjectModel;

namespace MoneyFlow.Application.Extension
{
    public static class CollectionExtension
    {
        public static void ReplaceCollection<T>(this IList<T> targetCollection, IEnumerable<T> values)
        {
            ArgumentNullException.ThrowIfNull(values);

            targetCollection.Clear();

            foreach (var item in values)
                targetCollection.Add(item);
        }

        public static void ReversList<T>(this ObservableCollection<T> collection)
        {
            if (collection == null || collection.Count < 2)
                return;

            var count = collection.Count;

            for (int i = 0; i < count - 1; i++)
                collection.Move(count - 1, i);
        }
    }
}
