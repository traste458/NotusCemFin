<%
  Dim sT
  if Request.ServerVariables("REMOTE_ADDR") = Request.ServerVariables("LOCAL_ADDR") Then
    sT = Request("SessionVar")
    if Trim(sT) <> "" Then
      Response.Write Session(sT)
    End If
  End If
%>
