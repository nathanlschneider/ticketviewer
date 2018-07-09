' owner.exe - Change Wendy's CFC store owner GUID.
Imports System.IO
Imports System.Net
Imports System.Threading

Module Module1

    Dim h As System.Net.IPHostEntry = Dns.GetHostEntry(Dns.GetHostName)
    Dim localIP As String = h.AddressList(0).ToString
    Dim storeStr() As String
    Dim siteNum As String
    Dim OWNERID(20000) As String
    Const AeMFilePath = "C:\BootDrv\CFC\Settings\AeMInStoreSettings.xml"
    Const CFCFilePath = "C:\BootDrv\CFC\Settings\CFCInStoreSettings.xml"
    Const AeMFilePathTemp = "C:\BootDrv\CFC\Settings\AeMInStoreSettings.tmp"
    Const CFCFilePathTemp = "C:\BootDrv\CFC\Settings\CFCInStoreSettings.tmp"
    Const path = "C:\MHGI_Logs\"
    Const ownerLog = path & "owner.log"
    Dim lines() As String
    Dim lines2() As String
    Dim openCheck As Boolean = False


    Sub Main()

        initCheck()

        Dim t As New Thread(AddressOf AsyncLog)
        t.Priority = Threading.ThreadPriority.Normal
        t.Start()

        Try
            File.Create(ownerLog).Dispose()
        Catch ex As Exception
            AsyncLog(ex.Message)
        End Try

        AsyncLog("Starting Owner.exe")

        doBackUps() 'backs up CFC files

        DoOwner() ' changes the owner GUID

        AsyncLog("Running force delete...")

        forceDelete() 'runs a force delete

        AsyncLog("Restarting AEM service...")
        Proc("stop", "aeminstoreservice")  '
        Proc("start", "aeminstoreservice") ' restarts aeminstoreservice

        AsyncLog("Restarting CTRsvr service...")
        Proc("stop", "ctlsvr")  '
        Proc("start", "ctlsvr") ' restarts control service

        AsyncLog("Running force export")
        forceExport() ' runs a force in store export

        File.Create(path & "ALLDONE").Dispose()
        Thread.Sleep(1000000)

    End Sub

    Private Sub doBackUps()

        File.Create(path & "BackupsStarting").Dispose()

        Try
            AsyncLog("Making a backup up AeMInStoreSettings.xml...")
            File.Copy(AeMFilePath, AeMFilePath & ".BAC")
            AsyncLog("Done.")
        Catch ex As Exception
            AsyncLog(ex.Message)
        End Try

        Try
            AsyncLog("Making a backup up CFCInStoreSettings.xml...")
            File.Copy(AeMFilePath, CFCFilePath & ".BAC")
            AsyncLog("Done.")
        Catch ex As Exception
            AsyncLog(ex.Message)
        End Try

        File.Create(path & "BackupsDone").Dispose()

    End Sub

    Private Sub DoOwner()

        ' OWNERID(XX) = "????????-?????-????-????-????????????"
        ' XX = Intl. Site Number
        ' ?? = Store Owner GUID

        File.Create(path & "OwnerIDStarting").Dispose()

        'WOT
        'OWNERID(1020) = "0dbd7cab-c333-4cdd-9baa-b77262001887"
        ''OWNERID(2) = "61156a3b-7456-420e-bf7b-bbf5f560a95a"
        ''OWNERID(35) = "0dbd7cab-c333-4cdd-9baa-b77262001887"
        ''OWNERID(44) = "0fd1c1fb-2838-4c4d-97e5-659ef8c594b4"
        ''OWNERID(75) = "bfdd5ca7-e507-4768-8041-9d50ebad524f"
        ''OWNERID(80) = "4f70331f-8341-4945-ad5c-163d3b2cf98d"
        ''OWNERID(108) = "ec3aa23d-1c3f-4cb5-9c44-3c9937dbc3da"
        ''OWNERID(118) = "79db70c6-971b-4d8b-8411-03885722aa21"
        ''OWNERID(163) = "cdc96d32-0fe2-4b58-9496-e51da9a2c032"
        ''OWNERID(182) = "44da9f42-75c9-4c8e-8313-9865d09a1025"
        ''OWNERID(191) = "530e7ae4-a02a-4ca8-9c87-a8f890edf74b"
        ''OWNERID(236) = "1aa7460c-cfd3-488b-8843-70fd06b4adcb"
        ''OWNERID(304) = "d5bd354f-144e-4515-82f2-183f7d3bf256"
        ''OWNERID(322) = "d4514532-2003-425d-a87f-60b1d7bcf988"
        ''OWNERID(323) = "983409d3-7c77-4af3-b735-9ab75c71b7d8"
        ''OWNERID(344) = "66fa2051-af81-474d-a11e-97b1b4088760"
        ''OWNERID(439) = "7a6bd68b-ed3d-4da3-b33e-e73620eddaaf"
        ''OWNERID(469) = "aa5576d0-be49-450e-972d-ff23b080f784"
        ''OWNERID(543) = "604324d2-d96a-4333-8416-a6683a2fb913"
        ''OWNERID(1014) = "68d0d22b-af0e-4e95-916c-429b6ba75ca8"
        'OWNERID(1039) = "9b751f65-99a3-47b5-8b17-f95dc3624322"
        'OWNERID(1293) = "aa153710-88b4-49a5-bc70-cdc347b3b837"
        'OWNERID(1389) = "ff185e67-623d-408b-a7e2-dde43aa408bb"
        'OWNERID(1417) = "7d784b6f-e416-47b9-8ea8-470bfa662842"
        'OWNERID(1422) = "05cca522-b2e1-4bb8-a332-7a46bf906fa4"
        'OWNERID(2957) = "088d72e4-5b17-42dd-bafd-1ca34e947606"
        'OWNERID(3022) = "f933d6cb-0b2e-4a9f-a701-4ac8bc584510"
        'OWNERID(3070) = "1947a82e-f0bb-4e40-990b-4dd2e4dcddbf"
        'OWNERID(3125) = "c56d7000-2478-40b9-889c-2bedd95dcd2e"
        'OWNERID(3576) = "d678f301-3158-4aa9-90fb-432b5a403719"
        'OWNERID(4530) = "217ca969-ef3f-4abb-96c8-1467f061286e"
        'OWNERID(4599) = "5990d3fe-4d12-493f-8563-7917b60195d6"
        'OWNERID(4832) = "4c8ba66b-491a-4ebb-80fb-c2b7672ec446"
        'OWNERID(4861) = "217d9f98-d519-4c8f-a2dd-681b0467f54a"
        'OWNERID(4891) = "388a872c-aa50-4284-adc4-f043c856cab4"
        'OWNERID(4955) = "f8f50513-27b5-49ab-81c6-2e8e5412854e"
        'OWNERID(4997) = "3ed31585-e25b-470a-92a9-76751fb9de0d"
        'OWNERID(5009) = "616788ec-c677-4457-9b5c-9151844595ee"
        'OWNERID(5019) = "1a475f3a-5d8c-4c2f-96f3-621d3ec878e7"
        'OWNERID(5087) = "2f3a36f3-3695-4747-9509-b0d05330b2c2"
        'OWNERID(5117) = "cfb72af1-5fb6-43e0-a3d5-4c40b5327470"
        'OWNERID(5244) = "8fdff612-d27e-4f09-ab68-f3ec86903bc4"
        'OWNERID(5367) = "4ab43c63-c0da-40a5-a8e0-dfcdcbe76896"
        'OWNERID(5788) = "81217810-26cd-4e54-be04-e242e56fbf4f"
        'OWNERID(5806) = "9451bb36-fd32-4651-aaae-2f25916f2fea"
        'OWNERID(7425) = "f151f47f-c521-4540-b8b0-ff90e55d026c"
        'OWNERID(8444) = "f69aa84b-344c-485f-ac4c-38a36b677ad7"
        'OWNERID(8860) = "c59c85ae-55ad-451f-b321-68ece578b95c"
        'OWNERID(9103) = "659e6576-6036-4b9a-9ef3-fa7f4f922bfe"
        'OWNERID(9119) = "a635f536-81e5-4f83-83d1-3c649c74266f"
        'OWNERID(9137) = "49995357-986b-40d4-a7f1-64066f0b5f85"
        'OWNERID(9138) = "906134f7-a8d2-4fd0-986f-60e57eaa4ff4"
        'OWNERID(9675) = "8b465e08-5321-4949-a375-ed4ab67a2ff7"
        'OWNERID(9681) = "9f6e0cf9-7c24-4fc2-b874-12714d4eecb7"
        'OWNERID(9704) = "5114f3f6-353b-4f0d-ab71-a71178da80fd"
        'OWNERID(9705) = "0ecab243-54ca-48ff-832c-58605873d6af"
        'OWNERID(9980) = "bc8088c4-aee4-40f9-8f5c-237a6a4679d6"
        'OWNERID(9981) = "88efa4e1-75e8-4782-a682-6454e03e8f8a"
        'OWNERID(9982) = "a833aaf6-b290-4c20-a7d5-f2840b5f6c83"

        ''Texas 
        'OWNERID(5472) = "5752f021-ef1f-498d-b70f-dafe8926054d"
        'OWNERID(11686) = "be767458-a776-4eaa-b101-778c16f7968f"
        'OWNERID(4817) = "9502099b-6668-4193-9E29-848ba01ee300"
        'OWNERID(9858) = "08b50d43-95c4-40d5-91aa-393F287E9ab3"
        'OWNERID(2883) = "b2a6addc-182f-4730-b836-f53137726afd"

        storeStr = localIP.Split(".")
        siteNum = storeStr(1) & storeStr(2)

        If siteNum.StartsWith("0") Then
            siteNum = siteNum.TrimStart("0")
        Else

        End If

        If storeStr(2) = "0" Then
            siteNum = siteNum & "0"
        End If

        AsyncLog("Writing the AeM file...")

        Try
            lines = IO.File.ReadAllLines(AeMFilePath)
            Dim sw As StreamWriter = New StreamWriter(AeMFilePathTemp)

            For Each line As String In lines

                If line.Contains("<StoreOwnerId>") Then
                    line = "<StoreOwnerId>" & OWNERID(siteNum) & "</StoreOwnerId>"
                End If
                AsyncLog(line)
            Next

        Catch ex As Exception

            AsyncLog(ex.Message)

        End Try

        AsyncLog("Done.")
        AsyncLog("Writing the CFC file...")

        Try
            lines2 = IO.File.ReadAllLines(CFCFilePath)
            Dim sw As StreamWriter = New StreamWriter(CFCFilePathTemp)

            For Each line2 As String In lines2

                If line2.Contains("<StoreOwnerId>") Then
                    line2 = "<StoreOwnerId>" & OWNERID(siteNum) & "</StoreOwnerId>"

                End If
                AsyncLog(line2)
            Next

        Catch ex As Exception
            AsyncLog(ex.Message)
            File.Create(path & "OwnerIDError").Dispose()
        End Try

        AsyncLog("Done.")
        AsyncLog(" Doing some stuffs...")

        Try
            File.Delete(AeMFilePath)
            File.Delete(CFCFilePath)
            File.Move(AeMFilePathTemp, AeMFilePath)
            File.Move(CFCFilePathTemp, CFCFilePath
        Catch ex As Exception
            AsyncLog(ex.Message)
            File.Create(path & "OwnerIDError").Dispose()
        End Try

        AsyncLog("Done.")

        File.Create(path & "OwnerIDDone").Dispose()

    End Sub

    Private Sub forceExport()

        File.Create(path & "ExportStarted").Dispose()

        Dim forceExpProc As New Process

        Try
            forceExpProc.StartInfo.UseShellExecute = False
            forceExpProc.StartInfo.WorkingDirectory = "c:\bootdrv\cfc\instorebins\"
            forceExpProc.StartInfo.CreateNoWindow = True
            forceExpProc.StartInfo.FileName = "c:\bootdrv\cfc\instorebins\aeminstoreprocessor.exe"
            forceExpProc.StartInfo.Arguments = "/forceexport"
            forceExpProc.StartInfo.RedirectStandardOutput = True
            forceExpProc.Start()

            AsyncLog(forceExpProc.StandardOutput.ReadToEnd())
            forceExpProc.WaitForExit()
            forceExpProc.Close()
            File.Create(path & "ExportDone").Dispose()
        Catch ex As Exception

            AsyncLog(ex.Message)
            File.Create(path & "ExportERROR").Dispose()

        End Try

    End Sub

    Private Sub forceDelete()

        File.Create(path & "DeleteStart").Dispose()

        Dim forceDelProc As New Process

        Try
            forceDelProc.StartInfo.UseShellExecute = False
            forceDelProc.StartInfo.WorkingDirectory = "c:\bootdrv\cfc\instoreinstaller\"
            forceDelProc.StartInfo.CreateNoWindow = True
            forceDelProc.StartInfo.FileName = "C:\BootDrv\CFC\InStoreInstaller\InStoreDatabaseInstaller\AeMInStoreDatabaseInstaller.exe"
            forceDelProc.StartInfo.Arguments = "/forcedelete"
            forceDelProc.StartInfo.RedirectStandardOutput = True
            forceDelProc.Start()

            AsyncLog(forceDelProc.StandardOutput.ReadToEnd())
            forceDelProc.WaitForExit()
            forceDelProc.Close()
            File.Create(path & "DeleteDone").Dispose()
        Catch ex As Exception

            AsyncLog(ex.Message)
            File.Create(path & "DeleteERROR").Dispose()
        End Try

    End Sub

    Private Sub Proc(ByRef a As String, ByRef b As String)

        Dim runProc As New Process

        Try
            runProc.StartInfo.UseShellExecute = False
            runProc.StartInfo.CreateNoWindow = True
            runProc.StartInfo.FileName = "net.exe"
            runProc.StartInfo.Arguments = a & " " & b
            runProc.StartInfo.RedirectStandardOutput = True
            runProc.Start()

            AsyncLog(runProc.StandardOutput.ReadToEnd())
            runProc.WaitForExit()
            runProc.Close()

        Catch ex As Exception

            AsyncLog(ex.Message)
        End Try

    End Sub

    Private Sub initCheck()
    End Sub

    Delegate Sub AsyncLogDelegate([logData] As String)

    Private Sub AsyncLog(ByVal [logData] As String)

        If Not File.Exists(ownerLog) Then
            Using sw As StreamWriter = File.CreateText(ownerLog)
                sw.WriteLine(Date.Now & " " & logData)
                sw.Close()
            End Using

        End If

        Do
            Try
                Using sw As StreamWriter = File.AppendText(ownerLog)
                    sw.WriteLine(Date.Now & " " & logData)
                    sw.Close()
                    openCheck = False
                End Using
            Catch
                openCheck = True
            End Try
        Loop Until openCheck = False

    End Sub

End Module
