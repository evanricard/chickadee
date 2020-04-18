﻿namespace chickadee.core

(*
APRS 101
5 APRS DATA IN THE AX.25 INFORMATION FIELD

APRS Data Type
Identifier
Every APRS packet contains an APRS Data Type Identifier (DTI). This
determines the format of the remainder of the data in the Information field, as
follows:
APRS Data

5 APRS DATA IN THE AX.25 INFORMATION FIELD
APRS Data Type
Identifier
Every APRS packet contains an APRS Data Type Identifier (DTI). This
determines the format of the remainder of the data in the Information field, as
follows:
APRS Data Type Identifiers

*)

module APRSDataFormats =

    type DataFormat =
        | CurrentMicEData
        | OldMicEData
        | PositionReportWithoutTimeStampOrUltimeter
        | PeetBrosWeatherStation
        | RawGPSDataOrUltimeter
        | Argelo
        | OldMicEButCurrentTMD700
        | Item
        | ShelterDataWithTime
        | InvalidOrTest
        | PositionReportWithTimestampNoMessaging
        | Message
        | Object
        | StationCapabilities
        | PositionReportWithoutTimeStampWithMessaging
        | StatusReport
        | Query
        | PositionReportWithTimestampWithMessaging
        | TelemetryReport
        | MaidenheadGridLocatorBeacon
        | WeatherReportWihtoutPosition
        | CurrentMicEDataNotUsedInTMD700
        | UserDefined
        | ThirdPartyTraffic
        | Unsupported
        override this.ToString() =
            match this with
            | CurrentMicEData                               -> "Current Mic-E Data"
            | OldMicEData                                   -> "Old Mic-E Data"
            | PositionReportWithoutTimeStampOrUltimeter     -> "Position Report without timestamp or Ultimeter station"
            | PeetBrosWeatherStation                        -> "Peet Bros Weather Stations"
            | RawGPSDataOrUltimeter                         -> "Raw GPS Data or Ultimeter station"
            | Argelo                                        -> "Argelo DFJr/MicroFinder"
            | OldMicEButCurrentTMD700                       -> "Old Mic-E Data (but Current data for TM-D700)"
            | Item                                          -> "Item"
            | ShelterDataWithTime                           -> "[Reserved — Shelter data with time]"
            | InvalidOrTest                                 -> "Invalid or Test" 
            | PositionReportWithTimestampNoMessaging        -> "Position with timestamp (no APRS messaging)"
            | Message                                       -> "Message"
            | Object                                        -> "Object"
            | StationCapabilities                           -> "Station Capabilities"
            | PositionReportWithoutTimeStampWithMessaging   -> "Position without timestamp (with APRS messaging)"
            | StatusReport                                  -> "Status Report"
            | Query                                         -> "Query"
            | PositionReportWithTimestampWithMessaging      -> "Position with timestamp (with APRS messaging)"
            | TelemetryReport                               -> "Telemetry Report"
            | MaidenheadGridLocatorBeacon                   -> "Maidenhead grid locator beacon (obsolete)"
            | WeatherReportWihtoutPosition                  -> "Weather Report (without position)"
            | CurrentMicEDataNotUsedInTMD700                -> "Current Mic-E Data (not used in TM-D700)"
            | UserDefined                                   -> "User Defined"
            | ThirdPartyTraffic                             -> "Third-party traffic"
            | Unsupported                                   -> "Unsupported"
        static member dataIdentifier (df) =
            match df with
            | CurrentMicEData                               -> "\x1C"
            | OldMicEData                                   -> "\x1D"
            | PositionReportWithoutTimeStampOrUltimeter     -> "!"
            | PeetBrosWeatherStation                        -> "#"
            | RawGPSDataOrUltimeter                         -> "$"
            | Argelo                                        -> "%"
            | OldMicEButCurrentTMD700                       -> "'"
            | Item                                          -> ")"
            | ShelterDataWithTime                           -> "+"
            | InvalidOrTest                                 -> "," 
            | PositionReportWithTimestampNoMessaging        -> "/"
            | Message                                       -> ":"
            | Object                                        -> ";"
            | StationCapabilities                           -> "<"
            | PositionReportWithoutTimeStampWithMessaging   -> "="
            | StatusReport                                  -> ">"
            | Query                                         -> "?"
            | PositionReportWithTimestampWithMessaging      -> "@"
            | TelemetryReport                               -> "T"
            | MaidenheadGridLocatorBeacon                   -> "["
            | WeatherReportWihtoutPosition                  -> "_"
            | CurrentMicEDataNotUsedInTMD700                -> "`"
            | UserDefined                                   -> "{"
            | ThirdPartyTraffic                             -> "}"
            | Unsupported                                   -> ""
