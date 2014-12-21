﻿using DarklandsBusinessObjects.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarklandsBusinessObjects.Objects
{
    public class Saint
    {
        // TODO: move to service and read from files?
        private static string[] s_clueBookDescriptions =
        {
            "St.ADRIAN [43v, 15-99df, 15%]: End +(7-15),Chr +(6-11), each weapon skill +(15-29).",
            "St.AGATHA [24v, 20-80df, 25%]: If praying for a man, End +(6-11). If praying for a woman, End +(12-23), Chr +(6-11).",
            "St.AGNES [19v, 10-99df, 25%]: Party must include at least one female for this prayer to succeed (even if none are involved in the prayer). Chr +(10-29), Local Rep at nearest city +(10-20).",
            "St.AIDAN [23v, 15-99df, 25%]: Woodwise +(25-29). In interactions with animals, this saint may prevent attacks.",
            "St.ALBERT THE GREAT [39v, 25-99df, 60%]: Int +(8-15), Per +(6-11), Alch +(30-44), SpkL +(15-29), R&W +(15-29).",
            "St.ALCUIN [39v, 15-99df, 60%]: Int +(6-11), SpkL +(20-39), R&W +(20-39). Prayers in encounters with government and/or nobles can improve chances of success.",
            "St.ALEXIS [28v, 10-99df, 40%]: Chr +(6-11), Local Rep at nearest city +(10-20). If party has less than 12pf, it receives 36-108pf.",
            "St.ANDREW [14v, 5-50df, 20%]: End +(6-11), Chr +(8-15).",
            "St.ANDREW THE TRIBUNE [37v, 20-99df, 15%]: wEdg +(15-29), wImp +(15-29), wPol +(15-29), wThr +(15-29), wBow +(15-29).", 
            "St.ANTHONY [37v, 20-99df, 15%]: End +(4-9), Str +(5-10), Chr +(1-10), SpkL +(10-19), R&W +(20-39). The saint can sometimes weaken demons, but only during interactions before battle.",
            "St.ANTHONY OF PADUA [59v, 10-99df, 65%]: Str +(4-7), Per +(6-11), Chr +(15-24), wImp +(10-19), SpkC +(20-29), SpkL +(10-19).",
            "St.ARNULF [65v, 15-99df, 65%]: Str +(4-7), Per +(6-11), Int +(4-7), each weapon skill +(10-19), SpkL +(6-11), R&W +(8-15), StrW +(6-11), Ride +(8-15).",
            "St.APOLLINARIUS [78v, 20-50df, 70%]: Str +(2-4), End +(8-15). [Helps with merchant encounter.]",
            "St.BARBARA [32v, 20-50df, 40%]: Artf +(15-29), wMsD +(20-39).",
            "St.BATHILDIS [21v, 15-60df, 20%]: Int +(4-8), Per +(3-6). This saint may help you escape from prison, but frequently it costs a large amount of money.",
            "St.BONIFACE [25v, 10-90df, 10%]: Chr +(8-15), SpkC +(8-15), each weapon skill +(10-19). During interaction, this saint may be able to purify certain evil sites.",
            "St.CATHERINE (OF ALEXANDRIA) [46v, 30-70df, 30%]: Chr +(8-15), Int +(8-15), SpkC +(40-99).",
            "St.CATHERINE OF SIENA [33v, 10-99df, 25%]: Chr +(1-15), Per +(1-15), SpkC +(10-29). In interactions she may provide insights into the attitudes or feelings of others.",
            "St.CECILIA [41v, 25-99df, 50%]: Chr +(3-10), if person praying has a musical instrument +(20-60) to local reputation. She can save a parry from suffocation, or allow them to survive without breathing. This is useful in a few special situations.",
            "St.CEOLWULF [23v, 15-99df, 25%]: Per +(3-5), Str +(6-11), Int +(3-5), SpkL +(6-11), each weapon skill +(6-11).",
            "St.CHARITY [40v, 10-50df, 50%]: wFll +(15-29), Chr +(6-11), Local Rep at nearest city +(10-20), and armor thickness against flame weapons increased by 10.",
            "St.CHRISTINA THE ASTONISHING [54v, 50-99df, 50%]: Agl +(8-15), Chr +(1-10). In interactions she can sometimes levitate the party over obstacles or away from trouble, but she tends to send the party to deserted or relatively empty places.",
            "St.CHRISTOPHER [31v, 15-99df, 25%]: wBow +(6-11), StrW +(10-19), WdWs +(20-39), Ride +(25-74).",
            "St.CLARE [33v, 30-70df, 20%]: heals Str 95%, heals End 95%. The saint can sometimes weaken demons, but only during interactions before battle.",
            "St.CLOTILDA [22v, 10-99df, 30%]: heals End 25%, heals Str 50%, Heal (skill) +(10-19), Chr +(6-11).",
            "St.COLMAN OF CLOYNE [34v, 15-99df, 55%]: Chr +(8-15), SpkC +(30-89), SpkL +(6-11), R&W +(10-19).",
            "St.COLUMBA [36v, 20-99df, 60%]: End +(8-15), Str +(6-13), Chr +(4-7), Per -(5-9), weapon currently in use skill +(10-19). Note that St. Columba temporarily reduces perception.",
            "St.COSMAS [15v, 15-75df, 30%]: heals End 10%, heals Str 10%, Heal (skill) +(40-79), Per +(4-7), and automatically gives knowledge of St. Damian.",
            "St.CRISPIN [28v, 25-99df, 35%]: Improves non-metal armor quality on both limbs and vitals by 10.",
            "St.CYPRIAN [54v, 25-99df, 50%]: Int +(6-12), Alch +(6-12). The saint can sometimes weaken demons, but only during interactions before battle.",
            "St.DAMIAN [30v, 25-80df, 40%]: heals End 100%, heals Str 30%, and automatically gives knowledge of St. Cosmos.",
            "St.DAVID [30v, 10-75df, 25%]: Int +(4-7), Chr +(1-6), Per +(6-11), Ride +(6-11), weapon currently in use skill +(8-15).",
            "St.DENIS [38v, 25-99df, 20%]: heals Str 100%, Chr +(15-44), SpkC +(25-49).",
            "St.DERFEL GADARN [57v, 20-60df, 45%]: heals End 70%, Str +(5-14), weapon currently in use skill +(20-59).",
            "St.DEVOTA [26v, 20-65df, 10%]: heals End 100%, heals Str 20%, Chr +(15-29).",
            "St.DISMAS [53v, 30-99df, 45%]: WdWs +(10-19), StrW +(20-39), Artf +(20-39), Agl +(10-19), Stlh +(10-19). He may help a party escape from prison as an interaction option.",
            "St.DOMINIC [29v, 20-99df, 15%]: Int +(10-19), SpkC +(10-19), SpkL +(10-19), R&W +(20-39). He can help the party deal with some travelling clergy (friars, hermits, etc.).",
            "St.DOROTHY OF MONTAU [52v, 20-99df, 50%]: Chr +(10-19), Per +(6-11), Heal (skill) +(10-19). If the person prayed for lacks an edged weapon, he or she receives a longsword. During interactions this saint may allow you to sense whether the other person is good or evil.",
            "St.DROGO [59v, 15-99df, 60%]: Per +(6-11), Heal (skill) +(10-19), WdWs +(20-39), and helps prevent ambushes for seven days.",
            "St.DUNSTAN [45v, 30-60df, 60%]: Per +(6-11), Artf+(25-74), StrW +(6-11), Alch +(5-14), weapon currently in use skill +(6-11), armor thick-ness improves one against flame weapons.",
            "St.DYMPHNA [21v, 20-75df, 10%]: heals End 30%, Agl +(4-7), Heal (skill) +(10-19), Stlh +(15-29). The saint can sometimes weaken demons, but only during interactions before battle.",
            "St.EDWARD THE CONFESSOR [5v, 20-99df, 10%]: End +(4-7), Int +(6-11), Per +(8-15), each weapon skill +(6-11), Ride +(8-15). Each character in the party whose virtue is below 20 has their virtue increased to W. In interactions with nobility, this saint is sometimes helpful.",
            "St.ELIGIUS [29v, 10-60df, 25%]: Artf +(25-74), Alch +(8-15). (There are no weapon or armor improvements.) ",
            "St.EMYDIUS [31v, 20-99df, 55%]: SpkC +(20-39), Chr +(15-29). During interaction, this saint may be able to purify certain evil sites.",
            "St.ENGELBERT [40v, 15-99df, 30%]: Chr +(5-9), Ride +(10-19), SpkC +(6-11), WdWs +(10-19).",
            "St.ERASMUS [32v, 15-50df, 40%]: Agl +(6-11), Per +(6-11), and armor becomes inpenetrable to flame weapons.",
            "St.ERIC [42v, 20-99df, 25%]: Str +(8-15), Chr +(4-7), SpkC -46-11), Ride +(6-11), weapon currently in use skill +(15-24). [May help in certain encounters]",
            "St.EUSTACE [41v, 30-99df, 30%]: wEdg +(6-11), wThr +(6-11), wBow +(6-11), Ride +(10-19), WdWs +(40-69).",
            "St.FELIX OF NOLA [37v, 25-99df, 30%]: Agl +(8-13), Stlh +(40-99), WdWs +(10-19). During interactions, this saint may help an imprisoned party escape.",
            "St.FINBAR [17v, 30-99df, 45%]: Chr +(8-15), Per +(6-11), SpkL +(10-19). [Helps with nightwatch encounters in cities.]",
            "St.FINNIAN [24v, 40-99df, 35%]: Agl +(8-15), Chr +(6-11), SpkC +(6-11), R&W +(6-11), StrW +(4-7). In addition, the party can cross impassable water for 24 hours.",
            "St.FLORIAN [17v, 25-99df, 35%]: Riding +(6-11), weapon currently in use skill +(6-11). If the nearest city is Austrian (Wien, Graz, Passau, Salzburg, Linz or Steyr) Local Rep +(15-25). In addition, the party can cross impassable water for 24 hours.",
            "St.FRANCIS OF ASSISI [10v, 10-60%, 1%]: heals End 30%, heals Str 10%, End +(6-11),Chr +(15-29), Per +(6-11), SpkC +(4-7), Heal (skill) +(10-19), Artf +(12-23), WdWs +(4-7).",
            "St.GABRIEL [56v, 30-75df, 45%]: Int +(10-29), Per +(10-29). This archangel and saint can sometimes clear the way of obstacles.",
            "St.GENEVIEVE, [73v, 40-99df, 75%]: heals End 30%, heals Scr 20%, and helps prevent ambushes for seven days.",
            "St.GEORGE [23v, 40-99df, 10%]: Str +(10-14), Agl +(5-9), each weapon skill +(25-49), Ride +(25-49).",
            "St.GERLAC [34v, 40-75df, 50%]: heals End 30%, heals Str 20%, Heal (skill) +(20-39), weapon currently in use skill +(15-24).",
            "St.GERTRUDE O'NIVELLES [26v, 35-99df, 30%]: Chr +(6-11), SpkL +(15-29), R&W +(15-29), WdWs +(15-29), Riding +(5-9). If the party is in the countryside, it is immediately moved to the outskirts of the nearest city.",
            "St.GILES [45v, 35-99df, 15%]: heals End 40%, heals Str 10%, Agl -(4-7), weapon currently in use skill -(15-24), Stlh +(30-59), StrW +(50-89), WdWs +(5-9). Note that St. Giles temporarily reduces agility and certain weapons skills.",
            "St.GILES OF PORTUGAL [64v, 15-99df, 60%]: Alch +(40-59), Artf +(10-19), Int +(5-9), Per +(15-24). In addition, each prayer causes a -1 wound to strength, and -1 to virtue, which do not wear off after 24 hours.",
            "St.GODEHARD [39v, 15-99df, 55%]: heals End 30%, SpkL +(20-39), R&W +(25-49).",
            "St.GODFREY [55v, 25-99df, 40%]: heals End 20%, SpkC +(15-29), Artf +(10-19), and helps prevent ambushes for seven days. During inter-actions this saint may prevent fights with other people.",
            "St.GOTTSCHALK [64v, 25-50df, 55%]: each weapon skill +(15-29), if nearest city is Wendish (Schleswig, Lubeck, Hamburg, Bremen, Rostock and Wismar) Local Rep +(20-40) .",
            "St.GREGORY THAUMATURGUS [74v, 30-99df, 80%]: Chr +(8-15), Alch +(10-19), SpkC +(10-19), Artf +(20-39). During interactions this saint may sometimes cause impressive miracles that solve desperate or difficult problems.",
            "St.HEDWIG [16v, 15-75df, 10%]: Heal (skill) +(20-29), if nearest city is Silesian (Frankfurt an der Oder, Breslau, Olmctz and Teschen) Local Rep +(20-40). During interactions this saint may help you perceive the thoughts of nobles or other leaders.",
            "St.HENRY [61v, 30-99df, 75%]: Int +(8-23), each weapon skill +(10-19).",
            "St.HERIBERT [30v, 20-99df, 35%]: Chr +(10-19), Str +(4-7). During interactions this saint may help you perceive evil in peasants.",
            "St.HERVE [47v, 15-99df, 40%]: Per +(15-29). During interactions this saint may allow you to sense whether the other person is good or evil.",
            "St.HILDEGARD [13v, 25-99df, 25%]: Per +(20-59). In a few inter-action situations, this saint allows you to glimpse something of the future.",
            "St.HUBERT [34v, 50-99df, 50%]: wPol +(15-29), wThr +(15-29), wBow +(15-29), wMsD +(15-29), Stlh +(25-49), WdWs +(40-79). This saint sometimes allows you to avoid trouble in woodland encounters.",
            "St.ILLTYD [63v, 20-99df, 65%]: Chr +(15-29), SpkC +(10-19), Ride +(5-9), each weapon skill +(10-19).",
            "St.ISIDORE [21v, 10-99df, 35%]: Chr +(8-15), SpkC +(5-9), StrW +(10-19). This saint may help interactions with farmers or peasants.",
            "St.ITA [85v, 10-99df, 70%]: heals End 50%, heals Str 100%, Chr +(8-15), Heal (skill) +(20-39).",
            "St.JAMES THE GREATER [25v, 12-50df, 40%]: heals End 20%, heals Str 10%, Str +(4-7), End +(4-7), Chr +(3-5), Virt +(20-34).",
            "St.JANUARIUS [66v, 35-75df, 66%]: In interactions with animals this saint may prevent an attack.",
            "St.JOHN OF BRIDLINGTON [36v, 10-99df, 15%]: if praying for a man, heals End 30% and heals Str 10%; if praying for a woman, heals End 100%, heals Str 20%, End +(5-9), Str +(4-7).",
            "St.JOHN CHRYSOSTOM [37v, 10-50df, 25%]: Chr +(8-15), Per -(6-11), SpkC +(30-59), SpkL +(30-59). Note that perception is actually reduced temporarily.",
            "St.JOHN CLIMACUS [20v, 40-99df, 30%]: each non-weapon skill increased by +(1-4), plus the target's current virtue.",
            "St.JOHN NEPOLMUCHEN [22v, 15-60df, 30%]: Chr +(8-15), Int +(6-11), SpkC +(20-39), StrW +(4-7). If nearest city is Prag Local Rep +(35-55), if nearest city is another in Bohemia (St. Joachimsthal, Bürglitz, Kuttenberg, Brünn, Olmütz) Local Rep +(10-20). During interactions, this saint may force people to tell the truth. This can be very helpful in a number of countryside encounters.",
            "St.JOSEPH [27v, 35-99df, 35%]: Chr +(6-11), Artf +(30-59), StrW +(30-59).",
            "St.JUDE [15v, 20-90df, 5%]: each attribute +(4-8). Weapon currently in use skill +(15-24), each non-weapon skill +(6-11). [Helps with desperate situations.]",
            "St.JULIAN THE HOSPITALER [61v, 40-99df, 75%]: Ride +(6-11), WdWs +(20-39), Also, the party can cross impassable water for 24 hours.",
            "St.KESSOG [31v, 20-99df, 25%]: heals End 40%, heals Str 20%, Chr +(6-11), SpkC +(15-29), Heal (skill) +(10-19), Ride +(4-7).",
            "St.LASDISLAUS [51v, 25-99df, 60%]: Str +(4-7), Agl +(3-5), SpkC +(6-11), Ride +(10-19).",
            "St.LAWRENCE [20v, 30-99df, 10%]: heals End 100%, Chr +(12-19), SpkC +(10-19), StrW +(10-19).",
            "St.LAZARUS [80v, 50-99df, 60%]: heals End 30%, heals Str 100%. This saint may help you cure the plague.",
            "St.LONGINIUS [49v, 30-99df, 45%]: each weapon skill +(10-19).",
            "St.LUCY [77v, 40-99df, 65%]: heals End 100%, heals Str 100%, Per +(6-11). [Helps with nightwatch encounters in cities.]",
            "St.LUKE [17v, 20-99df, 35%]: heals End 50%, heals Str 30%, Heal (skill) +(20-39).",
            "St.LUTGARDIS [60v, 35-99df, 50%]: Chr +(6-11), Per +(8-15), Virt +(6-17), End +(5-9). During some interactions, this saint may translate (fly) the party over obstacles such as walls.",
            "St.MARGARET [46v, 25-75df, 45%]: heals End 100%, heals Str 50%, each attribute +(3-6), each non-weapon skill +(5-10).",
            "St.MARGARET OF CORTONA [52v, 25-99df, 10%]: heals End 100%, heals Str 40%, Heal (skill) +(20-39), SpkC +(10-19).",
            "St.MARK [35v, 25-99df, 40%]: Agl +(12-23), R&W +(15-29).",
            "St.MARTIN OF TOURS [69v, 25-99df, 65%]: heals End 30%, heals Str 30%, Chr +(8-15), Per +(12-19), SpkC +(25-49), Ride +(20-39). During interactions this saint may allow you to sense whether the other person is good or evil.",
            "St.MATTHEW [26v, 20-80df, 25%]: Int +(8-15), SpkL +(15-29), R&W +(25-49).",
            "St.MAURICE [81v, 30-50df, 70%]: wEdg +(20-39), Alch +(10-19).",
            "St.MICHAEL [72v, 25-99df, 55%]: Heal (skill) +(20-39), Chr +(12-19), weapons currently in use +20.",
            "St.MILBURGA [71v, 35-99df, 70%]: Heal (skill) +(20-39), Chr +(10-19), for the next 24 hours allows the party to move over impassable water. During some interactions this saint may translate (fly) the party over obstacles such as walls.",
            "St.MOSES THE BLACK [66v, 10-99df, 65%]: each weapon skill +(15-29), Stlh +(20-39), WdWs +(10-19), StrW +(10-19).",
            "St.NICHOLAS [49v, 25-99df, 55%]: End +(4-8), Chr +(6-11), wFll +(15-29), Local Rep at nearest city +(10-20).",
            //"St.NICOLAS OF TOLENTINO [31v, 25.60df, 23%]: heals End 50%, heals Str 100%, Relg +(8-15), Heal (skill) +(12-23).",
            "St.ODILIA [25v, 20-99df, 40%]: Per +(6-11), if nearest city is in Alsace (Strassburg or Basel) then Local Rep +(15-25). [Helps with nightwatch encounters in cities.]",
            "St.ODO [47v, 30-99df, 60%]: Chr +(4-8), Per +(10-19), SpkC +(8-15), each weapon skill +(8-22).",
            "St.OLAF [18v, 15-99df, 20%]: heals End 30%, each weapon skill +(8-22).",
            "St.PANTALEON [56v, 25-99df, 20%]: heals End 30%, heals Str 30%, Chr +(6-11), Alch +(10-19), Heal (skill) +(15-29). Entire party armor thickness increased 10 against flame weapons. If the party is in the countryside, it is immediately moved to the outskirts of the nearest city. In interactions with animals this saint may prevent an attack.",
            "St.PATRICK [22v, 25-99df, 15%]: Chr +(15-24), Str +(12-19), wEdg +(15-29), wImp +(15-29), wPol +(15-29), SpkC +(15-29), SpkL +(15-29), R&W +(15-29), and armor thickness increased by 2 against flame weapons.",
            "St.PAUL THE APOSTLE [19v, 27-52df, 35%]: Int +(12-23), SpkC +(12-26), SpkL +(12-26), R&W +(15-29), Heal (skill) +(10-19).",
            "St.PAUL THE SIMPLE [70v, 20-80df, 60%]: heals End 100%, heals Str 100%, Int halved (temporarily). However, in some interactions, this saint may help you \"see into the mind\" of another.",
            "St.PERPETUA [16v, 40-99df, 10%]: Chr +(15-29). In interactions with animals this saint may prevent an attack.",
            "St.PETER [58v, 25-75df, 55%]: Str +(12-19), Chr +(8-15), SpkC +(10-19), SpkL +(10-19), Heal (skill) +(10-19), wEdg +(15-29), but Per halved (temporarily). If imprisoned, this saint may aid the party's escape.",
            "St.PETER OF ATROA [62v, 25-99df, 50%]: Int +(6-11), Per +(6-11), Alch +(10-19), Stlh +(40-79), Ride +(10-19). However, Local Rep in nearest city -(1-5).",
            "St.POLYCARP [19v, 10-75df, 20%]: Improves armor thickness by 11 and adds 90 to armor quality of entire party when attacked by flame weapons.",
            "St.RAPHAEL [75v, 40-60df, 70%]: heals End 100%, heals Str 100%, Agl +(8-15), Per +(6-11), Heal (skill) +(50-99). [Helps with nightwatch encounters in cities.]",
            "St.RAYMOND PENAFORT [67v, 10-99df, 50%]: Int +(15-29), R&W +(30-89). In addition, the party can cross impassable water for 24 hours. The patron saint of lawyers, this saint's ability to argue religious law can be useful in a few encounters.",
            "St.RAYMOND LULL [29v, 10-99df, 35%]: Int +(12-19), Alch +(20-39), R&W +(30-59). In addition, if End and/or Str are above 9, they are reduced tog (i.e., character receiving this benefit is also likely to suffer the equivalent of serious wounds).",
            "St.REINOLD [27v, 25-99df, 45%]: heals End 30%, Alch +(6-17), Artf +(35-69), Chr -(6-11). This saint is frequently useful in scaling walls.",
            "St.ROCH [19v, 40-90df, 45%]: heals End 20%, heals Str 20%, Heal (skill) +(10-19), Artf +(8-15). This saint may help you cure the plague.",
            "St.SABAS THE GOTH [64v, 25-99df, 50%]: heals End 100%, heals Str 100%, Chr +(10-19).",
            "St.SEBASTIAN [28v, 15-90df, 10%]: heals End 10%, heals Str 10%, Agl +(12-19), wBow +(20-39), Ride +(10-19). This saint may help you cure the plague.",
            "St.STANISLAUS [42v, 40-99df, 65%]: heals End 20%, heals Str 10%, Chr +(6-11), SpkC +(30-49), SpkL +(20-39).",
            "St.STEPHEN [27v, 20-99df, 30%]: Chr +(6-11), Int +(8-15), SpkC +(6-11), Ride +(10-19), weapon currently in use skill +(15-29), if party is in Pressburg Local Rep +(15-25).",
            "St.SWITHBERT [50v, 30-99df, 30%]: heals End 20%, heals Str 10%, Chr +(10-19), SpkC +(25-49).",
            "St.TARACHUS [50v, 40-99df, 30%]: heals End 40%, heals Str 20%, Chr +(6-11). In interactions with animals this saint may prevent an attack.",
            "St.THALELAEUS \"THE MERCIFUL\" [62v, 10-99df, 65%]: heals End 40%, heals Str 20%, Heal (skill) +(30-59).",
            "St.THEODORE TIRO [38v, 25-99df, 40%]: Chr +(6-11), End +(4-8), wFll +(15-29), weapon currently in use skill +(15-24).",
            "St.THOMAS THE APOSTLE [24v, 10-65df, 24%]: Str +(4-8), End +(4-8), SpkC +(4-8), Heal (skill) +(10-19), Artf +(12-23), WdWs +(4-7).",
            "St.THOMAS AQUINAS [68v, 15-99df, 65%]: Int +(12-23), SpkC +(12-26), SpkL +(12-26), R&W +(15-29), Heal (skill) +(10-19). [Helps with Devil's bridge -encounter.]",
            "St.VALENTINE [48v, 10-50df, 45%]: heals End 10%, heals Str 10%, Chr +(10-19), Stlh +(6-11), Heal (skill) +(6-11), SpkC +(6-11).",
            "St.VICTOR O'MARSEILLES [65v, 30-80df, 40%]: heals End 100%, heals Str 20%, Chr +(8-15), SpkC +(10-19), weapon currently in use skill +(15-24) ",
            "St.VITUS [48v, 40-99df, 65%]: Chr +(10-19), Agl +(12-23). If the parry is in the countryside, it is immediately moved to the outskirts of the nearest city.",
            "St.WENCESLAUS [44v, 35-99df, 60%]: Str +(6-11), Int +(8-13), Per +(6-11), Ride +(8-15), weapon currently in use skill +(8-15). If nearest city is in Bohemia (Prag, Ss. Joachimsthal, Burglit, Kuttenberg, Brunn, Olmutz) Local Rep +(15-24).",
            "St.WILFRID [25v, 15-65df, 15%]: heals End 20%, Chr +(6-11), SpkC +(20-39), Artf +(6-11), and helps prevent ambushes for seven days.",
            "St.WILLEHAD [35v, 20-99df, 35%]: Agl +(8-15), Ride +(10-19), WdWs +(8-15), and helps prevent ambushes for seven days.",
            "St.WILLEBALD [50v, 25-99df, 55%]: Chr +(10-19), SpkC +(25-49), WdWs +(6-11).",
            "St.WILLIBORORD [43v, 15-50df, 55%]: Chr +(10-19), SpkC +(20-39), WdWs +(8-15).",
            "St.WOLFGANG [24v, 20-99df, 35%]: Chr +(8-15), Int +(6-11), SpkC +(20-39), Ride +(6-11), WdWs +(6-11).",
            "St.ZITA [23v, 10-65df, 15%]: heals End 20%, heals Str 5%, Per +(6-11), StrW +(6-11). During interactions with servants, this saint may help you gain additional information."
        };

        public const int DESCRIPTION_SIZE = 360;

        public int Id { get; private set; }
        public string FullName { get; private set; }
        public string ShortName { get; private set; }
        public string Description { get; private set; }
        public string Clue { get; private set; }

        public override string ToString()
        {
            return "['0x" + Id.ToString("x")
                + "' '" + ShortName
                + "' '" + FullName
                //+ "' '" + Description
                + "']";
        }

        public Saint(int id, string longName, string shortName, string description)
        {
            Id = id;
            FullName = longName;
            ShortName = shortName;
            Description = description;
            Clue = s_clueBookDescriptions[id];
        }
    }
}
