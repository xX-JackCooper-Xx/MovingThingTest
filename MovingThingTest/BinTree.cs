using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovingThingTest
{
    public class BinTree
    {
        Cell? cell;
        int? data;
        BinTree leftNode;
        BinTree rightNode;
        int depth = 0;
        int lowest = 0;

        public BinTree(int? inp, Cell? cell)
        {
            data = inp;
            this.cell = cell;
            if (data != null)
            {
                NewNodes();
            }
        }
        private void NewNodes()
        {
            leftNode = new BinTree(null, null);
            rightNode = new BinTree(null, null);
        }
        public void Add(int i, Cell cell)
        {

            if (i <= data)
            {
                leftNode.Add(i, cell);
            }
            else if (i > data)
            {
                rightNode.Add(i, cell);
            }
            else if (data == null)
            {
                data = i;
                this.cell = cell;
                NewNodes();
            }
        }

        public void PopSmallest()
        {
            ReturnLowest().d

        }
        public Cell ReturnInOrder(int search)
        {

            if (leftNode.data != null)
            {
                leftNode.ReturnInOrder(search);
            }

            depth++;

            if(depth == search)
            {
                return cell;
            }

            if (rightNode.data != null)
            {
                rightNode.ReturnInOrder(search);
            }

            return null;
        }

        public BinTree ReturnLowest()
        {
            if (leftNode.data != null)
            {
                if (lowest == 1)
                {
                    return leftNode;
                }
                leftNode.ReturnLowest();
            }
            lowest = 1;

        }

        public void ReturnPreOrder()
        {
            Console.WriteLine(data);
            if (leftNode.data != null)
            {
                leftNode.ReturnPreOrder();
            }
            if (rightNode.data != null)
            {
                rightNode.ReturnPreOrder();
            }
        }

        public void ReturnPostOrder()
        {
            if (leftNode.data != null)
            {
                leftNode.ReturnPostOrder();
            }
            if (rightNode.data != null)
            {
                rightNode.ReturnPostOrder();
            }
            Console.WriteLine(data);
        }
    }
}
