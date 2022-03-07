using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface CPN_Data_SatisfactionHandler
{
    public List<BuildTypes> LikedInteractions();
    public List<BuildTypes> HatedInteractions();
}
