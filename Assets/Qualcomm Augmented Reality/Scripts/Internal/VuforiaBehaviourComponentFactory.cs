﻿/*==============================================================================
Copyright (c) 2013 QUALCOMM Austria Research Center GmbH.
All Rights Reserved.
Qualcomm Confidential and Proprietary
==============================================================================*/

using UnityEngine;

/// <summary>
/// Factory class that adds child class Behaviours
/// </summary>
public class VuforiaBehaviourComponentFactory : IBehaviourComponentFactory
{
    #region PUBLIC_METHODS

    public MaskOutAbstractBehaviour AddMaskOutBehaviour(GameObject gameObject)
    {
        return gameObject.AddComponent<MaskOutBehaviour>();
    }

    public VirtualButtonAbstractBehaviour AddVirtualButtonBehaviour(GameObject gameObject)
    {
        return gameObject.AddComponent<VirtualButtonBehaviour>();
    }

    public TurnOffAbstractBehaviour AddTurnOffBehaviour(GameObject gameObject)
    {
        return gameObject.AddComponent<TurnOffBehaviour>();
    }

    public ImageTargetAbstractBehaviour AddImageTargetBehaviour(GameObject gameObject)
    {
        return gameObject.AddComponent<ImageTargetBehaviour>();
    }

    public MarkerAbstractBehaviour AddMarkerBehaviour(GameObject gameObject)
    {
        return gameObject.AddComponent<MarkerBehaviour>();
    }

    public MultiTargetAbstractBehaviour AddMultiTargetBehaviour(GameObject gameObject)
    {
        return gameObject.AddComponent<MultiTargetBehaviour>();
    }

    public CylinderTargetAbstractBehaviour AddCylinderTargetBehaviour(GameObject gameObject)
    {
        return gameObject.AddComponent<CylinderTargetBehaviour>();
    }

    public WordAbstractBehaviour AddWordBehaviour(GameObject gameObject)
    {
        return gameObject.AddComponent<WordBehaviour>();
    }

    #endregion // PUBLIC_METHODS
}