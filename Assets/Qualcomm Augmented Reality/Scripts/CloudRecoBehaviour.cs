/*==============================================================================
Copyright (c) 2012-2013 QUALCOMM Austria Research Center GmbH.
All Rights Reserved.
Confidential and Proprietary - QUALCOMM Austria Research Center GmbH.
==============================================================================*/

using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the main behaviour class that encapsulates cloud recognition behaviour.
/// It just has to be added to a Vuforia-enabled Unity scene and will initialize the target finder and wait for new results.
/// State changes and new results will be sent to registered ICloudRecoEventHandlers
/// </summary> 
public class CloudRecoBehaviour : CloudRecoAbstractBehaviour
{
}