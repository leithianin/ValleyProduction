using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface CPN_Data_SatisfactionHandler
{
    public List<SatisfactorType> LikedInteractions();
    public List<SatisfactorType> HatedInteractions();
}
