module TNC2FormatTests

open Expecto
open chickadee.core.TNC2MON
open chickadee.core.Common
open chickadee.core.PositionReport
open chickadee.core.DataFormats.DataFormatType
open chickadee.core.APRSDataFormats

[<Literal>]
let SENDER = "kg7sio"
[<Literal>]
let DESTINATION = "apdw15" //DireWolf v1.5 ToCall
[<Literal>]
let PATH = "WIDE1-1"
let LATITUDE = 36.0591117 //DD decimal degrees
let LONGITUDE = -112.1093343 //DD decimal degrees
let LONGITUDE_HEMISPHERE = East // 'E'
let LATITUDE_HEMISPHERE =  North // 'N'
let POSITION_REPORT_HOUSE = sprintf "!%.2f%c/%.2f%c-" LATITUDE (LATITUDE_HEMISPHERE.ToHemisphereChar()) LONGITUDE (LONGITUDE_HEMISPHERE.ToHemisphereChar())
let FINAL_POSITION_REPORT = "!3603.33N/11206.34W-"
let TNC2_FINAL = (sprintf "%s>%s,%s:%s" (SENDER.ToUpper()) (DESTINATION.ToUpper()) PATH FINAL_POSITION_REPORT)


let PACKET_POSITION_REPORT_HOUSE =
    { 
        Position = { 
            Latitude = FormattedLatitude.create LATITUDE 
            Longitude = FormattedLongitude.create LONGITUDE 
        } 
        Symbol = House
        Comment = None //(PositionReportComment.create String.Empty).Value
        TimeStamp = None
    }

// TODO: introduce property based testsing?
// TODO: more tests for all position report types when possible
[<Tests>]
let TNC2MONFormatTests =
    testList "TNC2MON Format Tests" [
        testCase "Can build a packet with Position Report with latitude and longitude and upper call sign" <| fun _ ->
            let packet = 
                {
                    Sender      = (CallSign.create (SENDER.ToUpper())).Value
                    Destination = (CallSign.create (DESTINATION.ToUpper())).Value
                    Path        = WIDEnN WIDE11 //"WIDE1-1"
                    Information = Some (chickadee.core.PositionReport.PositionReportWithoutTimeStampOrUltimeter PACKET_POSITION_REPORT_HOUSE 
                                  |> Information.PositionReport)
                }.ToString()
            // Console.WriteLine packet
            Expect.equal packet TNC2_FINAL (sprintf "TNC2MON formats didnt match")
        testCase "Can build a packet with Position Report with latitude and longitude and lower callsign goes to upper" <| fun _ ->
            let packet = 
                {
                    Sender      = (CallSign.create SENDER).Value
                    Destination = (CallSign.create DESTINATION).Value
                    Path        = WIDEnN WIDE11 //"WIDE1-1"
                    Information = Some (chickadee.core.PositionReport.PositionReportWithoutTimeStampOrUltimeter PACKET_POSITION_REPORT_HOUSE 
                                  |> Information.PositionReport)
                }.ToString()
            // Console.WriteLine packet
            Expect.equal packet TNC2_FINAL (sprintf "TNC2 formats didnt match")
    ]

[<Tests>]
let RawPacketTypeTests =
    testList "Raw Packet Type Tests" [
        testCase "Current MicE Data" <| fun _ ->
            Expect.equal ((|FormatType|_|) "\x1C blaaaa").Value CurrentMicEData "CurrentMicEData"
        testCase "Old MicE Data" <| fun _ ->
            Expect.equal ((|FormatType|_|) "\x1D blaaa").Value OldMicEData "OldMicEData"
        testCase "Position Report Without TimeStamp Or Ultimeter" <| fun _ ->            
            match ((|FormatType|_|) "! blahblah").Value with
            | PostionReport t -> Expect.equal (t.ToString()) (PositionReportWithoutTimeStampOrUltimeter.ToString()) "PositionReportWithoutTimeStampOrUltimeter"
            | _ -> failwith "Expected a position report"
        testCase "Peet Bros Weather Station #" <| fun _ ->
            Expect.equal ((|FormatType|_|) "# blahblah").Value PeetBrosWeatherStation "PeetBrosWeatherStation"
        testCase "Raw GPS Data Or Ultimeter" <| fun _ ->
            Expect.equal ((|FormatType|_|) "$ blahblah").Value RawGPSDataOrUltimeter "RawGPSDataOrUltimeter"
        testCase "Argelo" <| fun _ ->
            Expect.equal ((|FormatType|_|) "% blahblah").Value Argelo "Argelo"
        testCase "Old MicE But Current TMD700" <| fun _ ->
            Expect.equal ((|FormatType|_|) "' blahblah").Value OldMicEButCurrentTMD700 "OldMicEButCurrentTMD700"
        testCase "Item" <| fun _ ->
            Expect.equal ((|FormatType|_|) ") blahblah").Value Item "Item"
        testCase "Peet Bros Weather Station *" <| fun _ ->
            Expect.equal ((|FormatType|_|) "* blahblah").Value PeetBrosWeatherStation "PeetBrosWeatherStation"
        testCase "Shelter Data With Time" <| fun _ ->
            Expect.equal ((|FormatType|_|) "+ blahblah").Value ShelterDataWithTime "ShelterDataWithTime"
        testCase "Invalid Or Test" <| fun _ ->
            Expect.equal ((|FormatType|_|) ", blahblah").Value InvalidOrTest "InvalidOrTest"
        testCase "Position Report With Timestamp No Messaging" <| fun _ ->
            match ((|FormatType|_|) "/ blahblah").Value with
            | PostionReport t -> Expect.equal (t.ToString()) (PositionReportWithTimestampNoMessaging.ToString()) "PositionReportWithTimestampNoMessaging"
            | _ -> failwith "Expected a position report"
        testCase "Message" <| fun _ ->
            Expect.equal ((|FormatType|_|) ": blahblah").Value Message "Message"
        testCase "Object" <| fun _ ->
            Expect.equal ((|FormatType|_|) "; blahblah").Value Object "Object"
        testCase "Station Capabilities" <| fun _ ->
            Expect.equal ((|FormatType|_|) "< blahblah").Value StationCapabilities "StationCapabilities"
        testCase "Position Report Without TimeStamp With Messaging" <| fun _ ->
            match ((|FormatType|_|) "= blahblah").Value with
            | PostionReport t -> Expect.equal (t.ToString()) (PositionReportWithoutTimeStampWithMessaging.ToString()) "PositionReportWithoutTimeStampWithMessaging"
            | _ -> failwith "Expected a position report"
        testCase "Status Report" <| fun _ ->
            Expect.equal ((|FormatType|_|) "> blahblah").Value StatusReport "StatusReport"
        testCase "Query" <| fun _ ->
            Expect.equal ((|FormatType|_|) "? blahblah").Value Query "Query"
        testCase "Position Report With Timestamp With Messaging" <| fun _ ->
            match ((|FormatType|_|) "@ blahblah").Value with
            | PostionReport t -> Expect.equal (t.ToString()) (PositionReportWithTimestampWithMessaging.ToString()) "PositionReportWithTimestampWithMessaging"
            | _ -> failwith "Expected a position report"
        testCase "T is Telemetry Report" <| fun _ ->
            Expect.equal ((|FormatType|_|) "T blahblah").Value TelemetryReport "TelemetryReport"
        testCase "[ Maidenhead Grid Locator Beacon" <| fun _ ->
            Expect.equal ((|FormatType|_|) "[ blahblah").Value MaidenheadGridLocatorBeacon "MaidenheadGridLocatorBeacon"
        testCase "_ Weather Report without position" <| fun _ ->
            Expect.equal ((|FormatType|_|) "_ blahblah").Value WeatherReportWihtoutPosition "WeatherReportWihtoutPosition"
        testCase "` Current MicE Data Not used In TMD 700" <| fun _ ->
            Expect.equal ((|FormatType|_|) "` blahblah").Value CurrentMicEDataNotUsedInTMD700 "CurrentMicEDataNotUsedInTMD700"
        testCase "{ User Defined" <| fun _ ->
            Expect.equal ((|FormatType|_|) "{ blahblah").Value UserDefined "UserDefined"
        testCase "} is Third Party" <| fun _ ->
            Expect.equal ((|FormatType|_|) "} blahblah").Value ThirdPartyTraffic "ThirdPartyTraffic"
        testCase "Unsupported character" <| fun _ ->
            Expect.equal ((|FormatType|_|) "N blahblah").Value Unsupported "Unsupported"
    ]

[<Tests>]
let DataIdentifierTests =
    testList "Raw Packet Data Identifier Tests" [
        testCase "PositionReportWithoutTimeStampOrUltimeter" <| fun _ ->
            Expect.equal (DataFormat.dataIdentifier (PostionReport PositionReportWithoutTimeStampOrUltimeter)) "!" "!" 
        testCase "PositionReportWithTimestampNoMessaging" <| fun _ ->
            Expect.equal (DataFormat.dataIdentifier (PostionReport PositionReportWithTimestampNoMessaging)) "/" "/" 
        testCase "PositionReportWithoutTimeStampWithMessaging" <| fun _ ->
            Expect.equal (DataFormat.dataIdentifier (PostionReport PositionReportWithoutTimeStampWithMessaging)) "=" "=" 
        testCase "PositionReportWithTimestampWithMessaging" <| fun _ ->
            Expect.equal (DataFormat.dataIdentifier (PostionReport PositionReportWithTimestampWithMessaging)) "@" "@" 
        testCase "CurrentMicEData" <| fun _ ->
            Expect.equal (DataFormat.dataIdentifier CurrentMicEData) "\x1C" "\x1C"
        testCase "OldMicEData" <| fun _ ->
            Expect.equal (DataFormat.dataIdentifier OldMicEData) "\x1D" "\x1D"
        testCase "PeetBrosWeatherStation" <| fun _ ->
            Expect.equal (DataFormat.dataIdentifier PeetBrosWeatherStation) "#" "#"
        testCase "RawGPSDataOrUltimeter" <| fun _ ->
            Expect.equal (DataFormat.dataIdentifier RawGPSDataOrUltimeter) "$" "$"
        testCase "Argelo" <| fun _ ->
            Expect.equal (DataFormat.dataIdentifier Argelo) "%" "%"
        testCase "OldMicEButCurrentTMD700" <| fun _ ->
            Expect.equal (DataFormat.dataIdentifier OldMicEButCurrentTMD700) "'" "'"
        testCase "Item" <| fun _ ->
            Expect.equal (DataFormat.dataIdentifier Item) ")" ")"
        testCase "ShelterDataWithTime" <| fun _ ->
            Expect.equal (DataFormat.dataIdentifier ShelterDataWithTime) "+" "+"
        testCase "InvalidOrTest" <| fun _ ->
            Expect.equal (DataFormat.dataIdentifier InvalidOrTest) "," ","
        testCase "Message" <| fun _ ->
            Expect.equal (DataFormat.dataIdentifier Message) ":" ":"
        testCase "Object" <| fun _ ->
            Expect.equal (DataFormat.dataIdentifier Object) ";" ";"
        testCase "StationCapabilities" <| fun _ ->
            Expect.equal (DataFormat.dataIdentifier StationCapabilities) "<" "<"
        testCase "Query" <| fun _ ->
            Expect.equal (DataFormat.dataIdentifier Query) "?" "?"
        testCase "TelemetryReport" <| fun _ ->
            Expect.equal (DataFormat.dataIdentifier TelemetryReport) "T" "T"
        testCase "MaidenheadGridLocatorBeacon" <| fun _ ->
            Expect.equal (DataFormat.dataIdentifier MaidenheadGridLocatorBeacon) "[" "["
        testCase "WeatherReportWihtoutPosition" <| fun _ ->
            Expect.equal (DataFormat.dataIdentifier WeatherReportWihtoutPosition) "_" "_"
        testCase "CurrentMicEDataNotUsedInTMD700" <| fun _ ->
            Expect.equal (DataFormat.dataIdentifier CurrentMicEDataNotUsedInTMD700) "`" "`"
        testCase "UserDefined" <| fun _ ->
            Expect.equal (DataFormat.dataIdentifier UserDefined) "{" "{"
        testCase "ThirdPartyTraffic" <| fun _ ->
            Expect.equal (DataFormat.dataIdentifier ThirdPartyTraffic) "}" "}"
        testCase "Unsupported" <| fun _ ->
            Expect.equal (DataFormat.dataIdentifier Unsupported) "" ""

    ]

