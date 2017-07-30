using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface Shape
{
    IEnumerable<AxialCoordinate> From(AxialCoordinate origin);
    IEnumerable<AxialCoordinate> FromOffset();
}
