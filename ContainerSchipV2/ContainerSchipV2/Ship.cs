using ContainerSchipV2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerSchipV2
{
    public class Ship
    {
        public IEnumerable<Place> Layout => Convert2DSLotArrayToIEnumerable(shipmanager.Layout);

        private ShipManager shipmanager;
        public int TotalMaxContainerWeight { get; private set; }
        public int Width { get; private set; }
        public int Length { get; private set; }
        public int TotalMinContainerWeight => TotalMaxContainerWeight / 2;
        public bool AbletoSail { get; private set; }
        public string Exception { get; private set; }


        public Ship(int width, int lenght)
        {
            if (width < 1 || lenght < 1)
            {
                throw new ArgumentException("lenght or width is to low");
            }
            Length = lenght;
            Width = width;
            shipmanager = new ShipManager(lenght, width);
            AbletoSail = false;
            Exception = "Ship not loaded yet";
        }

        public IEnumerable<Container> NotPlacedContainers => shipmanager.NotPlacedContainers;

        public void LoadShip(List<Container> containers)
        {
            shipmanager.GenerateLayout(containers);
            AbletoSailUpdate();
        }

        private void AbletoSailUpdate()
        {
            string exception = "";
            AbletoSail = true;
            double leftSidePercentage = (double)shipmanager.LeftSideWeight / ((double)shipmanager.LeftSideWeight + (double)shipmanager.RightSideWeight);
            if (leftSidePercentage < 0.4 || leftSidePercentage > 0.6)
            {
                AbletoSail = false;
                exception += "Ship not in balance";
            }

            if (shipmanager.TotalWeight < TotalMinContainerWeight)
            {
                if (AbletoSail == false)
                {
                    exception += ", ";
                }
                AbletoSail = false;
                exception += "Total weight of placed containers is lower than minimum needed Weight";
            }

            if (shipmanager.TotalWeight > TotalMaxContainerWeight)
            {
                if (AbletoSail == false)
                {
                    exception += ", ";
                }
                AbletoSail = false;
                exception += "Total weight of placed containers is higher than allowed maximum weight";
            }

            if (AbletoSail)
            {
                exception += "Everything is in order";
            }
            exception += ".";
            Exception = exception;
        }
        private IEnumerable<Place> Convert2DSLotArrayToIEnumerable(Place[][] place)
        {
            List<Place> returnIEnum = new List<Place>();
            foreach (Place[] secondArray in place)
            {
                foreach (Place places in secondArray)
                {
                    returnIEnum.Add(places);
                }
            }
            return returnIEnum;
        }

    }
}
