<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="VideoActiveX._Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title></title>
</head>
<body>

<%
    string ur = new UriBuilder(Request.Url.Scheme, Request.Url.Host, Request.Url.Port).ToString();
    string ur2 = ur + "VideoActiveX/TiTGActiveXControl.cab#version=1,0,0,0";
    string ur3 = ur + "VideoActiveX/psc-com.cer";
%>

        <a href="<%=ur3 %>">Download PSC Company Root Certificate (.cer file)</a><br />

        <object id="myVideo" name="myVideo" 
            codebase="<%=ur2 %>"
            classid="clsid:3F18C116-7BB6-46e4-A649-F6693E577002"
            width="554" height="358">

            <param name="BackgroundColor" value="transparent"/>
        </object>

            <!-- 
                codebase="<%=ur2 %>"
                classid="clsid:3F18C116-7BB6-46e4-A649-F6693E577002"
			    progid="TiTGActiveXControl.VideoControl"

                <param name="VisitorsDatabase" value="local" />
                <param name="ShowDeleteVisitorsButton" value="true" />
            -->


    <form id="form1" runat="server">
    <div>
        <input type="text" name="txt" value=""/>
        <input type="button" value="Start Video" onclick="startVideo();"/>
        <input type="button" value="Stop Video" onclick="stopVideo();"/>
    </div>
    </form>

</body>

<script type="text/javascript">
    //document.myVideo.Background = "#e8e8e8";

    window.onload = function () {
        //myVideo.StartAxVideoControl();
        //document.myVideo.Background = "#e8e8e8";
    }

    function startVideo() {
        myVideo.StartAxVideoControl();
        //document.myVideo.Background = "#e8e8e8";
        //myVideo.MyTitle = form1.txt.value;
        //document.myVideo.Open();
    }

    function stopVideo() {
        myVideo.StopAxVideoControl();
        //document.myVideo.Background = "#e8e8e8";
        //myVideo.MyTitle = form1.txt.value;
        //document.myVideo.Open();
    }
</script>

</html>
