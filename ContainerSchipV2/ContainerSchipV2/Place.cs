using ContainerSchipV2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerSchipV2
{
    public class Place
    {

        private readonly List<Container> Containers;
        public IEnumerable<Container> containers => Containers;
        public int PlaceXPostion { get; private set; }
        public int PlaceYPosition { get; private set; }
        public int MaxHeight { get; private set; }
        public int TotalWeight => Containers.Sum(c => c.Weight);

        public Place( int xPosition, int yPosition)
        {
            Containers = new List<Container>();
            PlaceXPostion = xPosition;
            PlaceYPosition = yPosition;
             
        }

        private bool CheckHeightLimit()
        {
            return Containers.Count < MaxHeight;
        }
        private bool CheckTopLoadFromBottom(Container container)
        {
            return TotalWeight <= container.MaxTopWeight;
        }
        public bool CanBePlacedAtBottom(Container newContainer)
        {
            return CheckTopLoadFromBottom(newContainer) && CheckHeightLimit();
        }
        public void PlaceAtBottom(Container container)
        {
            if (CanBePlacedAtBottom(container))
            {
                Containers.Insert(0, container);
                container.Placed = true;
            }
        }





        private int CalculateTopLoadFromIndex(int index)
        {
            int TopLoad = 0;
            for (int x = index + 1; x < Containers.Count; x++)
            {
                TopLoad += Containers[x].Weight;
            }
            return TopLoad;
        }


    }
}
