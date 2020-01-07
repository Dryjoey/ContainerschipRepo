using ContainerSchipV2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ContainerSchipV2.Enum;

namespace ContainerSchipV2
{
    internal class ShipManager
    {
        public Place[][] Layout => GetCombinedLayout();

        Place[][] left;
        Place[][] right;
        Place[] middle;

        private List<Container> _notPlacedContainers = new List<Container>();
        public IEnumerable<Container> NotPlacedContainers => _notPlacedContainers;
        public int Length { get; private set; }
        public int Width { get; private set; }
        public int TotalMaxLoad => Length * Width * 150000;
        public int TotalMinLoad => TotalMaxLoad / 2;
        public bool EvenWidth { get; private set; }
        public int LeftSideWeight => GetWeightFromCollection(left);
        public int RightSideWeight => GetWeightFromCollection(right);
        public int TotalWeight => GetWeightFromCollection(Layout);
        public int MiddleWeight => middle.Sum(c => c.TotalWeight);


        public ShipManager(int length, int width)
        {
            if (width < 1 || length < 1)
            {
                throw new ArgumentException("lenght or width is to low");
            }
            Length = length;
            Width = width;
            EvenWidth = width % 2 == 0;
            SetPlaces(EvenWidth);
        }
        private void TryAddContainer(Container container)
        {
            if (middle != null)
            {
                TryAddContainerToMiddle(container);
            }

            TryAddContainerToSide(container, GetSideWithLeastWeight());

            if (!container.Placed)
            {
                _notPlacedContainers.Add(container);
            }
        }
        public void GenerateLayout(List<Container> containers)
        {
            foreach (Container container in containers.OrderBy(c => c.Type).ThenByDescending(c => c.Weight))
            {
                TryAddContainer(container);
            }
        }



        private void TryAddContainerToMiddle(Container container)
        {
            TryAddContainerInPLace(container, middle);
        }

        private void TryAddContainerToSide(Container container, Place[][] side)
        {
            foreach (Place[] slotArray in side)
            {
                TryAddContainerInPLace(container, slotArray);
            }
        }


        private bool CheckIfValuableIsBlocked(Place CurrentPLace)
        {
            bool frontSave = true;
            bool backSave = true;

            if (CurrentPLace.PlaceYPosition > 0)
            {
                frontSave = CompareTwoPlacesIfValuableIsBlocked(CurrentPLace, Layout[CurrentPLace.PlaceXPostion][CurrentPLace.PlaceYPosition - 1]);
            }

            if (CurrentPLace.PlaceYPosition < Length - 2)
            {
                backSave = CompareTwoPlacesIfValuableIsBlocked(CurrentPLace, Layout[CurrentPLace.PlaceXPostion][CurrentPLace.PlaceYPosition + 1]);
            }

            return frontSave && backSave;

        }

        private bool CheckForSpecialPlacement(Container container, Place place)
        {
            if (container.Type == ContainerType.Cool)
            {
                return PlaceFirstRow(place);
            }
            else if (container.Type == ContainerType.Valuable)
            {
                return place.containers.Count() == 0 && !CheckForAdjecent(place);
            }
            else
            {
                return true;
            }

        }
        private void TryAddContainerInPLace(Container container, Place[] colomn)
        {
            if (container.Placed == false)
            {
                foreach (Place indivudualSlot in colomn)
                {
                    if (indivudualSlot.CanBePlacedAtBottom(container) && CheckIfValuableIsBlocked(indivudualSlot) && CheckForSpecialPlacement(container, indivudualSlot))
                    {
                        indivudualSlot.PlaceAtBottom(container);
                        break;
                    }
                }
            }
        }

        private bool CheckForAdjecent(Place place)
        {
            bool anyAdjecent = false;
            if (place.PlaceYPosition > 0)
            {
                if (Layout[place.PlaceXPostion][place.PlaceYPosition - 1].containers.Count() > 0)
                {
                    anyAdjecent = true;
                }
            }

            if (place.PlaceYPosition < Length - 2)
            {
                if (Layout[place.PlaceXPostion][place.PlaceYPosition + 1].containers.Count() > 0)
                {
                    anyAdjecent = true;
                }
            }
            return anyAdjecent;
        }


        private bool PlaceFirstRow(Place CurrentPlace)
        {
            return CurrentPlace.PlaceYPosition == 0;
        }


        private Place[][] GetSideWithLeastWeight()
        {
            return LeftSideWeight <= RightSideWeight ? left : right;
        }
        private bool CompareTwoPlacesIfValuableIsBlocked(Place CurrentPlace, Place OtherPlace)
        {
            if (OtherPlace.containers.Any(c => c.Type == ContainerType.Valuable))
            {
                int indexValuable = OtherPlace.containers.ToList().IndexOf(OtherPlace.containers.First(c => c.Type == ContainerType.Valuable));
                return indexValuable > CurrentPlace.containers.Count();
            }
            return true;
        }

        private Place[][] GetCombinedLayout()
        {
            if (Width > 1)
            {
                return CombineLayouts(EvenWidth);
            }
            else
            {
                return MiddleAsLayout();
            }
        }


        private int GetWeightFromCollection(Place[][] side)
        {
            int returnWeight = 0;
            foreach (Place[] slotArray in side)
            {
                foreach (Place place in slotArray)
                {
                    returnWeight += place.TotalWeight;
                }
            }
            return returnWeight;
        }
        private Place[][] MiddleAsLayout()
        {
            Place[][] combined = GetEmptyFullLayout();
            middle.CopyTo(combined[0], 0);
            return combined;
        }
        private Place[][] GetEmptyFullLayout()
        {
            Place[][] returnLayout = new Place[Width][];

            for (int i = 0; i < Width; i++)
            {
                returnLayout[i] = new Place[Length];
            }
            return returnLayout;
        }
        private Place[][] CombineLayouts(bool even)
        {
            Place[][] combined = GetEmptyFullLayout();

            left.CopyTo(combined, 0);

            if (even)
            {
                right.CopyTo(combined, Width / 2);
            }
            else
            {
                middle.CopyTo(combined[(Width - 1) / 2], 0);
                right.CopyTo(combined, Width / 2 + 1);
            }

            return combined;
        }


        private void FillLeftRightLayout()
        {
            for (int x = 0; x < Width / 2; x++)
            {
                left[x] = new Place[Length];
                right[x] = new Place[Length];

                for (int y = 0; y < Length; y++)
                {
                    left[x][y] = new Place( x, y);

                    right[x][y] = new Place(EvenWidth ? Width / 2 + x : Width / 2 + x + 1, y);
                }
            }
        }



        private void SetPlaces(bool evenWidth)
        {
            if (!evenWidth)
            {
                middle = new Place[Length];
                FillMiddleLayout();
            }

            if (Width > 1)
            {
                left = new Place[Width / 2][];
                right = new Place[Width / 2][];
                FillLeftRightLayout();
            }


        }
        private void FillMiddleLayout()
        {
            for (int i = 0; i < Length; i++)
            {
                middle[i] = new Place(Width / 2, i);
            }
        }

    }
}
