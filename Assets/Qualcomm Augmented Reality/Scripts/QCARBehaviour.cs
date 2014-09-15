/*==============================================================================
Copyright (c) 2010-2013 QUALCOMM Austria Research Center GmbH.
All Rights Reserved.
Confidential and Proprietary - QUALCOMM Austria Research Center GmbH.
==============================================================================*/

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using UnityEngine;

/// <summary>
/// The QCARBehaviour class handles tracking and triggers native video
/// background rendering. The class updates all Trackables in the scene.
/// </summary>
[RequireComponent(typeof(Camera))]
public class QCARBehaviour : QCARAbstractBehaviour
{
}
