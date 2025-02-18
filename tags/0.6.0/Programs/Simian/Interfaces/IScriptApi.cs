﻿using System;
using System.Collections.Generic;
using System.Text;
using OpenMetaverse;

using LSL_Float = Simian.ScriptTypes.LSL_Float;
using LSL_Integer = Simian.ScriptTypes.LSL_Integer;
using LSL_Key = Simian.ScriptTypes.LSL_Key;
using LSL_List = Simian.ScriptTypes.LSL_List;
using LSL_Rotation = Simian.ScriptTypes.LSL_Rotation;
using LSL_String = Simian.ScriptTypes.LSL_String;
using LSL_Vector = Simian.ScriptTypes.LSL_Vector;

namespace Simian
{
    #region Scripting Exceptions

    public class ScriptEventAbortException : Exception
    {
        public ScriptEventAbortException()
        {
        }
    }

    public class ScriptSelfDeleteException : Exception
    {
        public ScriptSelfDeleteException()
        {
        }
    }

    #endregion Scripting Exceptions

    public interface IScriptApi
    {
        void Start(ISceneProvider scene, SimulationObject hostObject, UUID scriptID, bool isGodMode, bool automaticLinkPermission);
        void Stop();

        void state(string newState);

        LSL_Integer llAbs(int i);
        LSL_Float llAcos(double val);
        void llAddToLandBanList(string avatar, double hours);
        void llAddToLandPassList(string avatar, double hours);
        void llAdjustSoundVolume(double volume);
        void llAllowInventoryDrop(int add);
        LSL_Float llAngleBetween(LSL_Rotation a, LSL_Rotation b);
        void llApplyImpulse(LSL_Vector force, int local);
        void llApplyRotationalImpulse(LSL_Vector force, int local);
        LSL_Float llAsin(double val);
        LSL_Float llAtan2(double x, double y);
        void llAttachToAvatar(int attachment);
        LSL_Key llAvatarOnSitTarget();
        LSL_Rotation llAxes2Rot(LSL_Vector fwd, LSL_Vector left, LSL_Vector up);
        LSL_Rotation llAxisAngle2Rot(LSL_Vector axis, double angle);
        LSL_Integer llBase64ToInteger(string str);
        LSL_String llBase64ToString(string str);
        void llBreakAllLinks();
        void llBreakLink(int linknum);
        LSL_Integer llCeil(double f);
        void llClearCameraParams();
        void llCloseRemoteDataChannel(string channel);
        LSL_Float llCloud(LSL_Vector offset);
        void llCollisionFilter(string name, string id, int accept);
        void llCollisionSound(string impact_sound, double impact_volume);
        void llCollisionSprite(string impact_sprite);
        LSL_Float llCos(double f);
        void llCreateLink(string target, int parent);
        LSL_List llCSV2List(string src);
        LSL_List llDeleteSubList(LSL_List src, int start, int end);
        LSL_String llDeleteSubString(string src, int start, int end);
        void llDetachFromAvatar();
        LSL_Vector llDetectedGrab(int number);
        LSL_Integer llDetectedGroup(int number);
        LSL_Key llDetectedKey(int number);
        LSL_Integer llDetectedLinkNumber(int number);
        LSL_String llDetectedName(int number);
        LSL_Key llDetectedOwner(int number);
        LSL_Vector llDetectedPos(int number);
        LSL_Rotation llDetectedRot(int number);
        LSL_Integer llDetectedType(int number);
        LSL_Vector llDetectedTouchBinormal(int index);
        LSL_Integer llDetectedTouchFace(int index);
        LSL_Vector llDetectedTouchNormal(int index);
        LSL_Vector llDetectedTouchPos(int index);
        LSL_Vector llDetectedTouchST(int index);
        LSL_Vector llDetectedTouchUV(int index);
        LSL_Vector llDetectedVel(int number);
        void llDialog(string avatar, string message, LSL_List buttons, int chat_channel);
        void llDie();
        LSL_String llDumpList2String(LSL_List src, string seperator);
        LSL_Integer llEdgeOfWorld(LSL_Vector pos, LSL_Vector dir);
        void llEjectFromLand(string pest);
        void llEmail(string address, string subject, string message);
        LSL_String llEscapeURL(string url);
        LSL_Rotation llEuler2Rot(LSL_Vector v);
        LSL_Float llFabs(double f);
        LSL_Integer llFloor(double f);
        void llForceMouselook(int mouselook);
        LSL_Float llFrand(double mag);
        LSL_Vector llGetAccel();
        LSL_Integer llGetAgentInfo(string id);
        LSL_String llGetAgentLanguage(string id);
        LSL_Vector llGetAgentSize(string id);
        LSL_Float llGetAlpha(int face);
        LSL_Float llGetAndResetTime();
        LSL_String llGetAnimation(string id);
        LSL_List llGetAnimationList(string id);
        LSL_Integer llGetAttached();
        LSL_List llGetBoundingBox(string obj);
        LSL_Vector llGetCameraPos();
        LSL_Rotation llGetCameraRot();
        LSL_Vector llGetCenterOfMass();
        LSL_Vector llGetColor(int face);
        LSL_String llGetCreator();
        LSL_String llGetDate();
        LSL_Float llGetEnergy();
        LSL_Vector llGetForce();
        LSL_Integer llGetFreeMemory();
        LSL_Vector llGetGeometricCenter();
        LSL_Float llGetGMTclock();
        LSL_Key llGetInventoryCreator(string item);
        LSL_Key llGetInventoryKey(string name);
        LSL_String llGetInventoryName(int type, int number);
        LSL_Integer llGetInventoryNumber(int type);
        LSL_Integer llGetInventoryPermMask(string item, int mask);
        LSL_Integer llGetInventoryType(string name);
        LSL_Key llGetKey();
        LSL_Key llGetLandOwnerAt(LSL_Vector pos);
        LSL_Key llGetLinkKey(int linknum);
        LSL_String llGetLinkName(int linknum);
        LSL_Integer llGetLinkNumber();
        LSL_Integer llGetListEntryType(LSL_List src, int index);
        LSL_Integer llGetListLength(LSL_List src);
        LSL_Vector llGetLocalPos();
        LSL_Rotation llGetLocalRot();
        LSL_Float llGetMass();
        void llGetNextEmail(string address, string subject);
        LSL_String llGetNotecardLine(string name, int line);
        LSL_Key llGetNumberOfNotecardLines(string name);
        LSL_Integer llGetNumberOfPrims();
        LSL_Integer llGetNumberOfSides();
        LSL_String llGetObjectDesc();
        LSL_List llGetObjectDetails(string id, LSL_List args);
        LSL_Float llGetObjectMass(string id);
        LSL_String llGetObjectName();
        LSL_Integer llGetObjectPermMask(int mask);
        LSL_Integer llGetObjectPrimCount(string object_id);
        LSL_Vector llGetOmega();
        LSL_Key llGetOwner();
        LSL_Key llGetOwnerKey(string id);
        LSL_List llGetParcelDetails(LSL_Vector pos, LSL_List param);
        LSL_Integer llGetParcelFlags(LSL_Vector pos);
        LSL_Integer llGetParcelMaxPrims(LSL_Vector pos, int sim_wide);
        LSL_Integer llGetParcelPrimCount(LSL_Vector pos, int category, int sim_wide);
        LSL_List llGetParcelPrimOwners(LSL_Vector pos);
        LSL_Integer llGetPermissions();
        LSL_Key llGetPermissionsKey();
        LSL_Vector llGetPos();
        LSL_List llGetPrimitiveParams(LSL_List rules);
        LSL_Integer llGetRegionAgentCount();
        LSL_Vector llGetRegionCorner();
        LSL_Integer llGetRegionFlags();
        LSL_Float llGetRegionFPS();
        LSL_String llGetRegionName();
        LSL_Float llGetRegionTimeDilation();
        LSL_Vector llGetRootPosition();
        LSL_Rotation llGetRootRotation();
        LSL_Rotation llGetRot();
        LSL_Vector llGetScale();
        LSL_String llGetScriptName();
        LSL_Integer llGetScriptState(string name);
        LSL_String llGetSimulatorHostname();
        LSL_Integer llGetStartParameter();
        LSL_Integer llGetStatus(int status);
        LSL_String llGetSubString(string src, int start, int end);
        LSL_Vector llGetSunDirection();
        LSL_String llGetTexture(int face);
        LSL_Vector llGetTextureOffset(int face);
        LSL_Float llGetTextureRot(int side);
        LSL_Vector llGetTextureScale(int side);
        LSL_Float llGetTime();
        LSL_Float llGetTimeOfDay();
        LSL_String llGetTimestamp();
        LSL_Vector llGetTorque();
        LSL_Integer llGetUnixTime();
        LSL_Vector llGetVel();
        LSL_Float llGetWallclock();
        void llGiveInventory(string destination, string inventory);
        void llGiveInventoryList(string destination, string category, LSL_List inventory);
        LSL_Integer llGiveMoney(string destination, int amount);
        void llGodLikeRezObject(string inventory, LSL_Vector pos);
        LSL_Float llGround(LSL_Vector offset);
        LSL_Vector llGroundContour(LSL_Vector offset);
        LSL_Vector llGroundNormal(LSL_Vector offset);
        void llGroundRepel(double height, int water, double tau);
        LSL_Vector llGroundSlope(LSL_Vector offset);
        LSL_String llHTTPRequest(string url, LSL_List parameters, string body);
        LSL_String llInsertString(string dst, int position, string src);
        void llInstantMessage(string user, string message);
        LSL_String llIntegerToBase64(int number);
        LSL_String llKey2Name(string id);
        LSL_String llList2CSV(LSL_List src);
        LSL_Float llList2Float(LSL_List src, int index);
        LSL_Integer llList2Integer(LSL_List src, int index);
        LSL_Key llList2Key(LSL_List src, int index);
        LSL_List llList2List(LSL_List src, int start, int end);
        LSL_List llList2ListStrided(LSL_List src, int start, int end, int stride);
        LSL_Rotation llList2Rot(LSL_List src, int index);
        LSL_String llList2String(LSL_List src, int index);
        LSL_Vector llList2Vector(LSL_List src, int index);
        LSL_Integer llListen(int channelID, string name, string ID, string msg);
        void llListenControl(int number, int active);
        void llListenRemove(int number);
        LSL_Integer llListFindList(LSL_List src, LSL_List test);
        LSL_List llListInsertList(LSL_List dest, LSL_List src, int start);
        LSL_List llListRandomize(LSL_List src, int stride);
        LSL_List llListReplaceList(LSL_List dest, LSL_List src, int start, int end);
        LSL_List llListSort(LSL_List src, int stride, int ascending);
        LSL_Float llListStatistics(int operation, LSL_List src);
        void llLoadURL(string avatar_id, string message, string url);
        LSL_Float llLog(double val);
        LSL_Float llLog10(double val);
        void llLookAt(LSL_Vector target, double strength, double damping);
        void llLoopSound(string sound, double volume);
        void llLoopSoundMaster(string sound, double volume);
        void llLoopSoundSlave(string sound, double volume);
        void llMakeExplosion(int particles, double scale, double vel, double lifetime, double arc, string texture, LSL_Vector offset);
        void llMakeFire(int particles, double scale, double vel, double lifetime, double arc, string texture, LSL_Vector offset);
        void llMakeFountain(int particles, double scale, double vel, double lifetime, double arc, int bounce, string texture, LSL_Vector offset, double bounce_offset);
        void llMakeSmoke(int particles, double scale, double vel, double lifetime, double arc, string texture, LSL_Vector offset);
        void llMapDestination(string simname, LSL_Vector pos, LSL_Vector look_at);
        LSL_String llMD5String(string src, int nonce);
        LSL_String llSHA1String(string src);
        void llMessageLinked(int linknum, int num, string str, string id);
        void llMinEventDelay(double delay);
        void llModifyLand(int action, int brush);
        LSL_Integer llModPow(int a, int b, int c);
        void llMoveToTarget(LSL_Vector target, double tau);
        void llOffsetTexture(double u, double v, int face);
        void llOpenRemoteDataChannel();
        LSL_Integer llOverMyLand(string id);
        void llOwnerSay(string msg);
        void llParcelMediaCommandList(LSL_List commandList);
        LSL_List llParcelMediaQuery(LSL_List aList);
        LSL_List llParseString2List(string str, LSL_List separators, LSL_List spacers);
        LSL_List llParseStringKeepNulls(string src, LSL_List seperators, LSL_List spacers);
        void llParticleSystem(LSL_List rules);
        void llPassCollisions(int pass);
        void llPassTouches(int pass);
        void llPlaySound(string sound, double volume);
        void llPlaySoundSlave(string sound, double volume);
        void llPointAt(LSL_Vector pos);
        LSL_Float llPow(double fbase, double fexponent);
        void llPreloadSound(string sound);
        void llPushObject(string target, LSL_Vector impulse, LSL_Vector ang_impulse, int local);
        void llRefreshPrimURL();
        void llRegionSay(int channelID, string text);
        void llReleaseCamera(string avatar);
        void llReleaseControls();
        void llRemoteDataReply(string channel, string message_id, string sdata, int idata);
        void llRemoteDataSetRegion();
        void llRemoteLoadScript(string target, string name, int running, int start_param);
        void llRemoteLoadScriptPin(string target, string name, int pin, int running, int start_param);
        void llRemoveFromLandBanList(string avatar);
        void llRemoveFromLandPassList(string avatar);
        void llRemoveInventory(string item);
        void llRemoveVehicleFlags(int flags);
        LSL_Key llRequestAgentData(string id, int data);
        LSL_Key llRequestInventoryData(string name);
        void llRequestPermissions(string agent, int perm);
        LSL_Key llRequestSimulatorData(string simulator, int data);
        void llResetLandBanList();
        void llResetLandPassList();
        void llResetOtherScript(string name);
        void llResetScript();
        void llResetTime();
        void llRezAtRoot(string inventory, LSL_Vector position, LSL_Vector velocity, LSL_Rotation rot, int param);
        void llRezObject(string inventory, LSL_Vector pos, LSL_Vector vel, LSL_Rotation rot, int param);
        LSL_Float llRot2Angle(LSL_Rotation rot);
        LSL_Vector llRot2Axis(LSL_Rotation rot);
        LSL_Vector llRot2Euler(LSL_Rotation r);
        LSL_Vector llRot2Fwd(LSL_Rotation r);
        LSL_Vector llRot2Left(LSL_Rotation r);
        LSL_Vector llRot2Up(LSL_Rotation r);
        void llRotateTexture(double rotation, int face);
        LSL_Rotation llRotBetween(LSL_Vector start, LSL_Vector end);
        void llRotLookAt(LSL_Rotation target, double strength, double damping);
        LSL_Integer llRotTarget(LSL_Rotation rot, double error);
        void llRotTargetRemove(int number);
        LSL_Integer llRound(double f);
        LSL_Integer llSameGroup(string agent);
        void llSay(int channelID, string text);
        void llScaleTexture(double u, double v, int face);
        LSL_Integer llScriptDanger(LSL_Vector pos);
        LSL_Key llSendRemoteData(string channel, string dest, int idata, string sdata);
        void llSensor(string name, string id, int type, double range, double arc);
        void llSensorRemove();
        void llSensorRepeat(string name, string id, int type, double range, double arc, double rate);
        void llSetAlpha(double alpha, int face);
        void llSetBuoyancy(double buoyancy);
        void llSetCameraAtOffset(LSL_Vector offset);
        void llSetCameraEyeOffset(LSL_Vector offset);
        void llSetCameraParams(LSL_List rules);
        void llSetClickAction(int action);
        void llSetColor(LSL_Vector color, int face);
        void llSetDamage(double damage);
        void llSetForce(LSL_Vector force, int local);
        void llSetForceAndTorque(LSL_Vector force, LSL_Vector torque, int local);
        void llSetHoverHeight(double height, int water, double tau);
        void llSetInventoryPermMask(string item, int mask, int value);
        void llSetLinkAlpha(int linknumber, double alpha, int face);
        void llSetLinkColor(int linknumber, LSL_Vector color, int face);
        void llSetLinkPrimitiveParams(int linknumber, LSL_List rules);
        void llSetLinkTexture(int linknumber, string texture, int face);
        void llSetLocalRot(LSL_Rotation rot);
        void llSetObjectDesc(string desc);
        void llSetObjectName(string name);
        void llSetObjectPermMask(int mask, int value);
        void llSetParcelMusicURL(string url);
        void llSetPayPrice(int price, LSL_List quick_pay_buttons);
        void llSetPos(LSL_Vector pos);
        void llSetPrimitiveParams(LSL_List rules);
        void llSetPrimURL(string url);
        void llSetRemoteScriptAccessPin(int pin);
        void llSetRot(LSL_Rotation rot);
        void llSetScale(LSL_Vector scale);
        void llSetScriptState(string name, int run);
        void llSetSitText(string text);
        void llSetSoundQueueing(int queue);
        void llSetSoundRadius(double radius);
        void llSetStatus(int status, int value);
        void llSetText(string text, LSL_Vector color, double alpha);
        void llSetTexture(string texture, int face);
        void llSetTextureAnim(int mode, int face, int sizex, int sizey, double start, double length, double rate);
        void llSetTimerEvent(double sec);
        void llSetTorque(LSL_Vector torque, int local);
        void llSetTouchText(string text);
        void llSetVehicleFlags(int flags);
        void llSetVehicleFloatParam(int param, LSL_Float value);
        void llSetVehicleRotationParam(int param, LSL_Rotation rot);
        void llSetVehicleType(int type);
        void llSetVehicleVectorParam(int param, LSL_Vector vec);
        void llShout(int channelID, string text);
        LSL_Float llSin(double f);
        void llSitTarget(LSL_Vector offset, LSL_Rotation rot);
        void llSleep(double sec);
        void llSound(string sound, double volume, int queue, int loop);
        void llSoundPreload(string sound);
        LSL_Float llSqrt(double f);
        void llStartAnimation(string anim);
        void llStopAnimation(string anim);
        void llStopHover();
        void llStopLookAt();
        void llStopMoveToTarget();
        void llStopPointAt();
        void llStopSound();
        LSL_Integer llStringLength(string str);
        LSL_String llStringToBase64(string str);
        LSL_String llStringTrim(string src, int type);
        LSL_Integer llSubStringIndex(string source, string pattern);
        void llTakeCamera(string avatar);
        void llTakeControls(int controls, int accept, int pass_on);
        LSL_Float llTan(double f);
        LSL_Integer llTarget(LSL_Vector position, double range);
        void llTargetOmega(LSL_Vector axis, double spinrate, double gain);
        void llTargetRemove(int number);
        void llTeleportAgentHome(string agent);
        void llTextBox(string avatar, string message, int chat_channel);
        LSL_String llToLower(string source);
        LSL_String llToUpper(string source);
        void llTriggerSound(string sound, double volume);
        void llTriggerSoundLimited(string sound, double volume, LSL_Vector top_north_east, LSL_Vector bottom_south_west);
        LSL_String llUnescapeURL(string url);
        void llUnSit(string id);
        LSL_Float llVecDist(LSL_Vector a, LSL_Vector b);
        LSL_Float llVecMag(LSL_Vector v);
        LSL_Vector llVecNorm(LSL_Vector v);
        void llVolumeDetect(int detect);
        LSL_Float llWater(LSL_Vector offset);
        void llWhisper(int channelID, string text);
        LSL_Vector llWind(LSL_Vector offset);
        LSL_String llXorBase64Strings(string str1, string str2);
        LSL_String llXorBase64StringsCorrect(string str1, string str2);
    }
}
