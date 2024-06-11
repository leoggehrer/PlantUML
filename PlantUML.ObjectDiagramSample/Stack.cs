namespace PlantUML.ObjectDiagramSample
{
    public class Stack
    {
        #region nested class
        private class Element
        {
            public Element(double data, Element? next)
            {
                Data = data;
                Next = next;
            }
            public double Data { get; set; }
            public Element? Next { get; set; }
        }
        #endregion  nested class

        #region  fields
        private Element? _head = null;
        #endregion fields

        #region  properties
        public bool IsEmpty
        {
            get
            {
                return _head == null;
            }
        }
        #endregion properties

        #region constructors
        public Stack()
        {
            ObjectDiagram.Create(this, "title Stack after create instance");
        }
        #endregion constructors

        #region  methods
        public void Push(double data)
        {
            _head = new Element(data, _head);
            ObjectDiagram.Create(this, $"title Stack after Push({data})");
        }
        public double Pop()
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("Stack is empty!");
            }

            Element tmp = _head!;

            _head = tmp.Next;
            ObjectDiagram.Create(this, $"title Stack after Pop() => {tmp.Data}");
            return tmp.Data;
        }
        #endregion methods
    }
}
