import React from 'react'
import { WiMoonNew } from "react-icons/wi";
import { WiMoonWaxingCrescent1 } from "react-icons/wi";
import { WiMoonFirstQuarter } from "react-icons/wi";
import { WiMoonWaxingGibbous6 } from "react-icons/wi";
import { WiMoonFull } from "react-icons/wi";
import { WiMoonWaningGibbous1 } from "react-icons/wi";
import { WiMoonThirdQuarter } from "react-icons/wi";
import { WiMoonWaningCrescent5 } from "react-icons/wi";

const MoonPhase = ({ phase }) => {
    switch (phase) {
        case "New moon":
            return <WiMoonNew />
        case "Waxing crescent":
            return <WiMoonWaxingCrescent1  />
        case "First quarter":
            return <WiMoonFirstQuarter />
        case "Waxing gibbous":
            return <WiMoonWaxingGibbous6 />
        case "Full moon":
            return <WiMoonFull />
        case "Vaning gibbous":
            return <WiMoonWaningGibbous1 />
        case "Third quarter":
            return <WiMoonThirdQuarter />
        case "Vaning crescent":
            return <WiMoonWaningCrescent5 />
        default:
            return <></>;
    }
}

export default MoonPhase