using System;
using System.Collections.Generic;
using System.Text;
using static ContainerSchipV2.Enum;

namespace ContainerSchipV2
{
    public class Container
    {
        public ContainerType Type { get; private set; }
        public int Weight { get; private set; }
        public int MaxTopWeight => 120000;
        public bool Placed { get; set; }

        public Container(int weight, ContainerType type)
        {
            if (weight < 4000 || weight > 30000)
            {
                throw new System.ArgumentException("weight is to high or to low");
            }
            else
            {
                Weight = weight;
                Type = type;
                Placed = false;
            }
           
        }

        public override string ToString()
        {
            return $"Weight: {this.Weight}. Type: {this.Type}. Placed: {this.Placed}. MaxTopWeight: {this.MaxTopWeight}.";
        }
    }
}
