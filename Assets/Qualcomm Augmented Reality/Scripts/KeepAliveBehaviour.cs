/*==============================================================================
Copyright (c) 2013 QUALCOMM Austria Research Center GmbH.
All Rights Reserved.
Confidential and Proprietary - QUALCOMM Austria Research Center GmbH.
==============================================================================*/

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// The KeepAliveBehaviour allows Vuforia objects to be reused across multiple
/// scenes. This makes it possible to share datasets and targets between scenes.
/// </summary>
[RequireComponent(typeof (QCARBehaviour))]
public class KeepAliveBehaviour : KeepAliveAbstractBehaviour
{
}