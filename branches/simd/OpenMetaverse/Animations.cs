/*
 * Copyright (c) 2007-2008, openmetaverse.org
 * All rights reserved.
 *
 * - Redistribution and use in source and binary forms, with or without
 *   modification, are permitted provided that the following conditions are met:
 *
 * - Redistributions of source code must retain the above copyright notice, this
 *   list of conditions and the following disclaimer.
 * - Neither the name of the openmetaverse.org nor the names
 *   of its contributors may be used to endorse or promote products derived from
 *   this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
 * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
 * ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE
 * LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
 * SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
 * INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
 * CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
 * POSSIBILITY OF SUCH DAMAGE.
 */

using System;

namespace OpenMetaverse
{
    /// <summary>
    /// Static pre-defined animations available to all agents
    /// </summary>
    public static class Animations
    {
        /// <summary>Agent with afraid expression on face</summary>
        public readonly static Guid AFRAID = new Guid("6b61c8e8-4747-0d75-12d7-e49ff207a4ca");
        /// <summary>Agent aiming a bazooka (right handed)</summary>
        public readonly static Guid AIM_BAZOOKA_R = new Guid("b5b4a67d-0aee-30d2-72cd-77b333e932ef");
        /// <summary>Agent aiming a bow (left handed)</summary>
        public readonly static Guid AIM_BOW_L = new Guid("46bb4359-de38-4ed8-6a22-f1f52fe8f506");
        /// <summary>Agent aiming a hand gun (right handed)</summary>
        public readonly static Guid AIM_HANDGUN_R = new Guid("3147d815-6338-b932-f011-16b56d9ac18b");
        /// <summary>Agent aiming a rifle (right handed)</summary>
        public readonly static Guid AIM_RIFLE_R = new Guid("ea633413-8006-180a-c3ba-96dd1d756720");
        /// <summary>Agent with angry expression on face</summary>
        public readonly static Guid ANGRY = new Guid("5747a48e-073e-c331-f6f3-7c2149613d3e");
        /// <summary>Agent hunched over (away)</summary>
        public readonly static Guid AWAY = new Guid("fd037134-85d4-f241-72c6-4f42164fedee");
        /// <summary>Agent doing a backflip</summary>
        public readonly static Guid BACKFLIP = new Guid("c4ca6188-9127-4f31-0158-23c4e2f93304");
        /// <summary>Agent laughing while holding belly</summary>
        public readonly static Guid BELLY_LAUGH = new Guid("18b3a4b5-b463-bd48-e4b6-71eaac76c515");
        /// <summary>Agent blowing a kiss</summary>
        public readonly static Guid BLOW_KISS = new Guid("db84829b-462c-ee83-1e27-9bbee66bd624");
        /// <summary>Agent with bored expression on face</summary>
        public readonly static Guid BORED = new Guid("b906c4ba-703b-1940-32a3-0c7f7d791510");
        /// <summary>Agent bowing to audience</summary>
        public readonly static Guid BOW = new Guid("82e99230-c906-1403-4d9c-3889dd98daba");
        /// <summary>Agent brushing himself/herself off</summary>
        public readonly static Guid BRUSH = new Guid("349a3801-54f9-bf2c-3bd0-1ac89772af01");
        /// <summary>Agent in busy mode</summary>
        public readonly static Guid BUSY = new Guid("efcf670c-2d18-8128-973a-034ebc806b67");
        /// <summary>Agent clapping hands</summary>
        public readonly static Guid CLAP = new Guid("9b0c1c4e-8ac7-7969-1494-28c874c4f668");
        /// <summary>Agent doing a curtsey bow</summary>
        public readonly static Guid COURTBOW = new Guid("9ba1c942-08be-e43a-fb29-16ad440efc50");
        /// <summary>Agent crouching</summary>
        public readonly static Guid CROUCH = new Guid("201f3fdf-cb1f-dbec-201f-7333e328ae7c");
        /// <summary>Agent crouching while walking</summary>
        public readonly static Guid CROUCHWALK = new Guid("47f5f6fb-22e5-ae44-f871-73aaaf4a6022");
        /// <summary>Agent crying</summary>
        public readonly static Guid CRY = new Guid("92624d3e-1068-f1aa-a5ec-8244585193ed");
        /// <summary>Agent unanimated with arms out (e.g. setting appearance)</summary>
        public readonly static Guid CUSTOMIZE = new Guid("038fcec9-5ebd-8a8e-0e2e-6e71a0a1ac53");
        /// <summary>Agent re-animated after set appearance finished</summary>
        public readonly static Guid CUSTOMIZE_DONE = new Guid("6883a61a-b27b-5914-a61e-dda118a9ee2c");
        /// <summary>Agent dancing</summary>
        public readonly static Guid DANCE1 = new Guid("b68a3d7c-de9e-fc87-eec8-543d787e5b0d");
        /// <summary>Agent dancing</summary>
        public readonly static Guid DANCE2 = new Guid("928cae18-e31d-76fd-9cc9-2f55160ff818");
        /// <summary>Agent dancing</summary>
        public readonly static Guid DANCE3 = new Guid("30047778-10ea-1af7-6881-4db7a3a5a114");
        /// <summary>Agent dancing</summary>
        public readonly static Guid DANCE4 = new Guid("951469f4-c7b2-c818-9dee-ad7eea8c30b7");
        /// <summary>Agent dancing</summary>
        public readonly static Guid DANCE5 = new Guid("4bd69a1d-1114-a0b4-625f-84e0a5237155");
        /// <summary>Agent dancing</summary>
        public readonly static Guid DANCE6 = new Guid("cd28b69b-9c95-bb78-3f94-8d605ff1bb12");
        /// <summary>Agent dancing</summary>
        public readonly static Guid DANCE7 = new Guid("a54d8ee2-28bb-80a9-7f0c-7afbbe24a5d6");
        /// <summary>Agent dancing</summary>
        public readonly static Guid DANCE8 = new Guid("b0dc417c-1f11-af36-2e80-7e7489fa7cdc");
        /// <summary>Agent on ground unanimated</summary>
        public readonly static Guid DEAD = new Guid("57abaae6-1d17-7b1b-5f98-6d11a6411276");
        /// <summary>Agent boozing it up</summary>
        public readonly static Guid DRINK = new Guid("0f86e355-dd31-a61c-fdb0-3a96b9aad05f");
        /// <summary>Agent with embarassed expression on face</summary>
        public readonly static Guid EMBARRASSED = new Guid("514af488-9051-044a-b3fc-d4dbf76377c6");
        /// <summary>Agent with afraid expression on face</summary>
        public readonly static Guid EXPRESS_AFRAID = new Guid("aa2df84d-cf8f-7218-527b-424a52de766e");
        /// <summary>Agent with angry expression on face</summary>
        public readonly static Guid EXPRESS_ANGER = new Guid("1a03b575-9634-b62a-5767-3a679e81f4de");
        /// <summary>Agent with bored expression on face</summary>
        public readonly static Guid EXPRESS_BORED = new Guid("214aa6c1-ba6a-4578-f27c-ce7688f61d0d");
        /// <summary>Agent crying</summary>
        public readonly static Guid EXPRESS_CRY = new Guid("d535471b-85bf-3b4d-a542-93bea4f59d33");
        /// <summary>Agent showing disdain (dislike) for something</summary>
        public readonly static Guid EXPRESS_DISDAIN = new Guid("d4416ff1-09d3-300f-4183-1b68a19b9fc1");
        /// <summary>Agent with embarassed expression on face</summary>
        public readonly static Guid EXPRESS_EMBARRASSED = new Guid("0b8c8211-d78c-33e8-fa28-c51a9594e424");
        /// <summary>Agent with frowning expression on face</summary>
        public readonly static Guid EXPRESS_FROWN = new Guid("fee3df48-fa3d-1015-1e26-a205810e3001");
        /// <summary>Agent with kissy face</summary>
        public readonly static Guid EXPRESS_KISS = new Guid("1e8d90cc-a84e-e135-884c-7c82c8b03a14");
        /// <summary>Agent expressing laughgter</summary>
        public readonly static Guid EXPRESS_LAUGH = new Guid("62570842-0950-96f8-341c-809e65110823");
        /// <summary>Agent with open mouth</summary>
        public readonly static Guid EXPRESS_OPEN_MOUTH = new Guid("d63bc1f9-fc81-9625-a0c6-007176d82eb7");
        /// <summary>Agent with repulsed expression on face</summary>
        public readonly static Guid EXPRESS_REPULSED = new Guid("f76cda94-41d4-a229-2872-e0296e58afe1");
        /// <summary>Agent expressing sadness</summary>
        public readonly static Guid EXPRESS_SAD = new Guid("eb6ebfb2-a4b3-a19c-d388-4dd5c03823f7");
        /// <summary>Agent shrugging shoulders</summary>
        public readonly static Guid EXPRESS_SHRUG = new Guid("a351b1bc-cc94-aac2-7bea-a7e6ebad15ef");
        /// <summary>Agent with a smile</summary>
        public readonly static Guid EXPRESS_SMILE = new Guid("b7c7c833-e3d3-c4e3-9fc0-131237446312");
        /// <summary>Agent expressing surprise</summary>
        public readonly static Guid EXPRESS_SURPRISE = new Guid("728646d9-cc79-08b2-32d6-937f0a835c24");
        /// <summary>Agent sticking tongue out</summary>
        public readonly static Guid EXPRESS_TONGUE_OUT = new Guid("835965c6-7f2f-bda2-5deb-2478737f91bf");
        /// <summary>Agent with big toothy smile</summary>
        public readonly static Guid EXPRESS_TOOTHSMILE = new Guid("b92ec1a5-e7ce-a76b-2b05-bcdb9311417e");
        /// <summary>Agent winking</summary>
        public readonly static Guid EXPRESS_WINK = new Guid("da020525-4d94-59d6-23d7-81fdebf33148");
        /// <summary>Agent expressing worry</summary>
        public readonly static Guid EXPRESS_WORRY = new Guid("9c05e5c7-6f07-6ca4-ed5a-b230390c3950");
        /// <summary>Agent falling down</summary>
        public readonly static Guid FALLDOWN = new Guid("666307d9-a860-572d-6fd4-c3ab8865c094");
        /// <summary>Agent walking (feminine version)</summary>
        public readonly static Guid FEMALE_WALK = new Guid("f5fc7433-043d-e819-8298-f519a119b688");
        /// <summary>Agent wagging finger (disapproval)</summary>
        public readonly static Guid FINGER_WAG = new Guid("c1bc7f36-3ba0-d844-f93c-93be945d644f");
        /// <summary>I'm not sure I want to know</summary>
        public readonly static Guid FIST_PUMP = new Guid("7db00ccd-f380-f3ee-439d-61968ec69c8a");
        /// <summary>Agent in superman position</summary>
        public readonly static Guid FLY = new Guid("aec4610c-757f-bc4e-c092-c6e9caf18daf");
        /// <summary>Agent in superman position</summary>
        public readonly static Guid FLYSLOW = new Guid("2b5a38b2-5e00-3a97-a495-4c826bc443e6");
        /// <summary>Agent greeting another</summary>
        public readonly static Guid HELLO = new Guid("9b29cd61-c45b-5689-ded2-91756b8d76a9");
        /// <summary>Agent holding bazooka (right handed)</summary>
        public readonly static Guid HOLD_BAZOOKA_R = new Guid("ef62d355-c815-4816-2474-b1acc21094a6");
        /// <summary>Agent holding a bow (left handed)</summary>
        public readonly static Guid HOLD_BOW_L = new Guid("8b102617-bcba-037b-86c1-b76219f90c88");
        /// <summary>Agent holding a handgun (right handed)</summary>
        public readonly static Guid HOLD_HANDGUN_R = new Guid("efdc1727-8b8a-c800-4077-975fc27ee2f2");
        /// <summary>Agent holding a rifle (right handed)</summary>
        public readonly static Guid HOLD_RIFLE_R = new Guid("3d94bad0-c55b-7dcc-8763-033c59405d33");
        /// <summary>Agent throwing an object (right handed)</summary>
        public readonly static Guid HOLD_THROW_R = new Guid("7570c7b5-1f22-56dd-56ef-a9168241bbb6");
        /// <summary>Agent in static hover</summary>
        public readonly static Guid HOVER = new Guid("4ae8016b-31b9-03bb-c401-b1ea941db41d");
        /// <summary>Agent hovering downward</summary>
        public readonly static Guid HOVER_DOWN = new Guid("20f063ea-8306-2562-0b07-5c853b37b31e");
        /// <summary>Agent hovering upward</summary>
        public readonly static Guid HOVER_UP = new Guid("62c5de58-cb33-5743-3d07-9e4cd4352864");
        /// <summary>Agent being impatient</summary>
        public readonly static Guid IMPATIENT = new Guid("5ea3991f-c293-392e-6860-91dfa01278a3");
        /// <summary>Agent jumping</summary>
        public readonly static Guid JUMP = new Guid("2305bd75-1ca9-b03b-1faa-b176b8a8c49e");
        /// <summary>Agent jumping with fervor</summary>
        public readonly static Guid JUMP_FOR_JOY = new Guid("709ea28e-1573-c023-8bf8-520c8bc637fa");
        /// <summary>Agent point to lips then rear end</summary>
        public readonly static Guid KISS_MY_BUTT = new Guid("19999406-3a3a-d58c-a2ac-d72e555dcf51");
        /// <summary>Agent landing from jump, finished flight, etc</summary>
        public readonly static Guid LAND = new Guid("7a17b059-12b2-41b1-570a-186368b6aa6f");
        /// <summary>Agent laughing</summary>
        public readonly static Guid LAUGH_SHORT = new Guid("ca5b3f14-3194-7a2b-c894-aa699b718d1f");
        /// <summary>Agent landing from jump, finished flight, etc</summary>
        public readonly static Guid MEDIUM_LAND = new Guid("f4f00d6e-b9fe-9292-f4cb-0ae06ea58d57");
        /// <summary>Agent sitting on a motorcycle</summary>
        public readonly static Guid MOTORCYCLE_SIT = new Guid("08464f78-3a8e-2944-cba5-0c94aff3af29");
        /// <summary></summary>
        public readonly static Guid MUSCLE_BEACH = new Guid("315c3a41-a5f3-0ba4-27da-f893f769e69b");
        /// <summary>Agent moving head side to side</summary>
        public readonly static Guid NO = new Guid("5a977ed9-7f72-44e9-4c4c-6e913df8ae74");
        /// <summary>Agent moving head side to side with unhappy expression</summary>
        public readonly static Guid NO_UNHAPPY = new Guid("d83fa0e5-97ed-7eb2-e798-7bd006215cb4");
        /// <summary>Agent taunting another</summary>
        public readonly static Guid NYAH_NYAH = new Guid("f061723d-0a18-754f-66ee-29a44795a32f");
        /// <summary></summary>
        public readonly static Guid ONETWO_PUNCH = new Guid("eefc79be-daae-a239-8c04-890f5d23654a");
        /// <summary>Agent giving peace sign</summary>
        public readonly static Guid PEACE = new Guid("b312b10e-65ab-a0a4-8b3c-1326ea8e3ed9");
        /// <summary>Agent pointing at self</summary>
        public readonly static Guid POINT_ME = new Guid("17c024cc-eef2-f6a0-3527-9869876d7752");
        /// <summary>Agent pointing at another</summary>
        public readonly static Guid POINT_YOU = new Guid("ec952cca-61ef-aa3b-2789-4d1344f016de");
        /// <summary>Agent preparing for jump (bending knees)</summary>
        public readonly static Guid PRE_JUMP = new Guid("7a4e87fe-de39-6fcb-6223-024b00893244");
        /// <summary>Agent punching with left hand</summary>
        public readonly static Guid PUNCH_LEFT = new Guid("f3300ad9-3462-1d07-2044-0fef80062da0");
        /// <summary>Agent punching with right hand</summary>
        public readonly static Guid PUNCH_RIGHT = new Guid("c8e42d32-7310-6906-c903-cab5d4a34656");
        /// <summary>Agent acting repulsed</summary>
        public readonly static Guid REPULSED = new Guid("36f81a92-f076-5893-dc4b-7c3795e487cf");
        /// <summary>Agent trying to be Chuck Norris</summary>
        public readonly static Guid ROUNDHOUSE_KICK = new Guid("49aea43b-5ac3-8a44-b595-96100af0beda");
        /// <summary>Rocks, Paper, Scissors 1, 2, 3</summary>
        public readonly static Guid RPS_COUNTDOWN = new Guid("35db4f7e-28c2-6679-cea9-3ee108f7fc7f");
        /// <summary>Agent with hand flat over other hand</summary>
        public readonly static Guid RPS_PAPER = new Guid("0836b67f-7f7b-f37b-c00a-460dc1521f5a");
        /// <summary>Agent with fist over other hand</summary>
        public readonly static Guid RPS_ROCK = new Guid("42dd95d5-0bc6-6392-f650-777304946c0f");
        /// <summary>Agent with two fingers spread over other hand</summary>
        public readonly static Guid RPS_SCISSORS = new Guid("16803a9f-5140-e042-4d7b-d28ba247c325");
        /// <summary>Agent running</summary>
        public readonly static Guid RUN = new Guid("05ddbff8-aaa9-92a1-2b74-8fe77a29b445");
        /// <summary>Agent appearing sad</summary>
        public readonly static Guid SAD = new Guid("0eb702e2-cc5a-9a88-56a5-661a55c0676a");
        /// <summary>Agent saluting</summary>
        public readonly static Guid SALUTE = new Guid("cd7668a6-7011-d7e2-ead8-fc69eff1a104");
        /// <summary>Agent shooting bow (left handed)</summary>
        public readonly static Guid SHOOT_BOW_L = new Guid("e04d450d-fdb5-0432-fd68-818aaf5935f8");
        /// <summary>Agent cupping mouth as if shouting</summary>
        public readonly static Guid SHOUT = new Guid("6bd01860-4ebd-127a-bb3d-d1427e8e0c42");
        /// <summary>Agent shrugging shoulders</summary>
        public readonly static Guid SHRUG = new Guid("70ea714f-3a97-d742-1b01-590a8fcd1db5");
        /// <summary>Agent in sit position</summary>
        public readonly static Guid SIT = new Guid("1a5fe8ac-a804-8a5d-7cbd-56bd83184568");
        /// <summary>Agent in sit position (feminine)</summary>
        public readonly static Guid SIT_FEMALE = new Guid("b1709c8d-ecd3-54a1-4f28-d55ac0840782");
        /// <summary>Agent in sit position (generic)</summary>
        public readonly static Guid SIT_GENERIC = new Guid("245f3c54-f1c0-bf2e-811f-46d8eeb386e7");
        /// <summary>Agent sitting on ground</summary>
        public readonly static Guid SIT_GROUND = new Guid("1c7600d6-661f-b87b-efe2-d7421eb93c86");
        /// <summary>Agent sitting on ground</summary>
        public readonly static Guid SIT_GROUND_staticRAINED = new Guid("1a2bd58e-87ff-0df8-0b4c-53e047b0bb6e");
        /// <summary></summary>
        public readonly static Guid SIT_TO_STAND = new Guid("a8dee56f-2eae-9e7a-05a2-6fb92b97e21e");
        /// <summary>Agent sleeping on side</summary>
        public readonly static Guid SLEEP = new Guid("f2bed5f9-9d44-39af-b0cd-257b2a17fe40");
        /// <summary>Agent smoking</summary>
        public readonly static Guid SMOKE_IDLE = new Guid("d2f2ee58-8ad1-06c9-d8d3-3827ba31567a");
        /// <summary>Agent inhaling smoke</summary>
        public readonly static Guid SMOKE_INHALE = new Guid("6802d553-49da-0778-9f85-1599a2266526");
        /// <summary></summary>
        public readonly static Guid SMOKE_THROW_DOWN = new Guid("0a9fb970-8b44-9114-d3a9-bf69cfe804d6");
        /// <summary>Agent taking a picture</summary>
        public readonly static Guid SNAPSHOT = new Guid("eae8905b-271a-99e2-4c0e-31106afd100c");
        /// <summary>Agent standing</summary>
        public readonly static Guid STAND = new Guid("2408fe9e-df1d-1d7d-f4ff-1384fa7b350f");
        /// <summary>Agent standing up</summary>
        public readonly static Guid STANDUP = new Guid("3da1d753-028a-5446-24f3-9c9b856d9422");
        /// <summary>Agent standing</summary>
        public readonly static Guid STAND_1 = new Guid("15468e00-3400-bb66-cecc-646d7c14458e");
        /// <summary>Agent standing</summary>
        public readonly static Guid STAND_2 = new Guid("370f3a20-6ca6-9971-848c-9a01bc42ae3c");
        /// <summary>Agent standing</summary>
        public readonly static Guid STAND_3 = new Guid("42b46214-4b44-79ae-deb8-0df61424ff4b");
        /// <summary>Agent standing</summary>
        public readonly static Guid STAND_4 = new Guid("f22fed8b-a5ed-2c93-64d5-bdd8b93c889f");
        /// <summary>Agent stretching</summary>
        public readonly static Guid STRETCH = new Guid("80700431-74ec-a008-14f8-77575e73693f");
        /// <summary>Agent in stride (fast walk)</summary>
        public readonly static Guid STRIDE = new Guid("1cb562b0-ba21-2202-efb3-30f82cdf9595");
        /// <summary>Agent surfing</summary>
        public readonly static Guid SURF = new Guid("41426836-7437-7e89-025d-0aa4d10f1d69");
        /// <summary>Agent acting surprised</summary>
        public readonly static Guid SURPRISE = new Guid("313b9881-4302-73c0-c7d0-0e7a36b6c224");
        /// <summary>Agent striking with a sword</summary>
        public readonly static Guid SWORD_STRIKE = new Guid("85428680-6bf9-3e64-b489-6f81087c24bd");
        /// <summary>Agent talking (lips moving)</summary>
        public readonly static Guid TALK = new Guid("5c682a95-6da4-a463-0bf6-0f5b7be129d1");
        /// <summary>Agent throwing a tantrum</summary>
        public readonly static Guid TANTRUM = new Guid("11000694-3f41-adc2-606b-eee1d66f3724");
        /// <summary>Agent throwing an object (right handed)</summary>
        public readonly static Guid THROW_R = new Guid("aa134404-7dac-7aca-2cba-435f9db875ca");
        /// <summary>Agent trying on a shirt</summary>
        public readonly static Guid TRYON_SHIRT = new Guid("83ff59fe-2346-f236-9009-4e3608af64c1");
        /// <summary>Agent turning to the left</summary>
        public readonly static Guid TURNLEFT = new Guid("56e0ba0d-4a9f-7f27-6117-32f2ebbf6135");
        /// <summary>Agent turning to the right</summary>
        public readonly static Guid TURNRIGHT = new Guid("2d6daa51-3192-6794-8e2e-a15f8338ec30");
        /// <summary>Agent typing</summary>
        public readonly static Guid TYPE = new Guid("c541c47f-e0c0-058b-ad1a-d6ae3a4584d9");
        /// <summary>Agent walking</summary>
        public readonly static Guid WALK = new Guid("6ed24bd8-91aa-4b12-ccc7-c97c857ab4e0");
        /// <summary>Agent whispering</summary>
        public readonly static Guid WHISPER = new Guid("7693f268-06c7-ea71-fa21-2b30d6533f8f");
        /// <summary>Agent whispering with fingers in mouth</summary>
        public readonly static Guid WHISTLE = new Guid("b1ed7982-c68e-a982-7561-52a88a5298c0");
        /// <summary>Agent winking</summary>
        public readonly static Guid WINK = new Guid("869ecdad-a44b-671e-3266-56aef2e3ac2e");
        /// <summary>Agent winking</summary>
        public readonly static Guid WINK_HOLLYWOOD = new Guid("c0c4030f-c02b-49de-24ba-2331f43fe41c");
        /// <summary>Agent worried</summary>
        public readonly static Guid WORRY = new Guid("9f496bd2-589a-709f-16cc-69bf7df1d36c");
        /// <summary>Agent nodding yes</summary>
        public readonly static Guid YES = new Guid("15dd911d-be82-2856-26db-27659b142875");
        /// <summary>Agent nodding yes with happy face</summary>
        public readonly static Guid YES_HAPPY = new Guid("b8c8b2a3-9008-1771-3bfc-90924955ab2d");
        /// <summary>Agent floating with legs and arms crossed</summary>
        public readonly static Guid YOGA_FLOAT = new Guid("42ecd00b-9947-a97c-400a-bbc9174c7aeb");
    }
}
