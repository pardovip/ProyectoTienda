Imports System.Runtime.InteropServices

Public Class FormularioPrincipal

#Region "Codigo Formulario"
    'RESIZE DEL FORMULARIO- CAMBIAR TAMAÑO'
    Dim cGrip As Integer = 10
    Protected Overrides Sub WndProc(ByRef m As Message)
        If (m.Msg = 132) Then
            Dim pos As Point = New Point((m.LParam.ToInt32 And 65535), (m.LParam.ToInt32 + 16))
            pos = Me.PointToClient(pos)
            If ((pos.X _
                    >= (Me.ClientSize.Width - cGrip)) _
                    AndAlso (pos.Y _
                    >= (Me.ClientSize.Height - cGrip))) Then
                m.Result = CType(17, IntPtr)
                Return
            End If
        End If
        MyBase.WndProc(m)
    End Sub
    '----------------DIBUJAR RECTANGULO / EXCLUIR ESQUINA PANEL' 
    Dim sizeGripRectangle As Rectangle
    Dim tolerance As Integer = 15
    Protected Overrides Sub OnSizeChanged(ByVal e As EventArgs)
        MyBase.OnSizeChanged(e)
        Dim region = New Region(New Rectangle(0, 0, Me.ClientRectangle.Width, Me.ClientRectangle.Height))
        sizeGripRectangle = New Rectangle((Me.ClientRectangle.Width - tolerance), (Me.ClientRectangle.Height - tolerance), tolerance, tolerance)
        region.Exclude(sizeGripRectangle)
        Me.PanelContenedor.Region = region
        Me.Invalidate()
    End Sub
    '----------------COLOR Y GRIP DE RECTANGULO INFERIOR'
    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Dim blueBrush As SolidBrush = New SolidBrush(Color.FromArgb(244, 244, 244))
        e.Graphics.FillRectangle(blueBrush, sizeGripRectangle)
        MyBase.OnPaint(e)
        ControlPaint.DrawSizeGrip(e.Graphics, Color.Transparent, sizeGripRectangle)
    End Sub


    <DllImport("user32.DLL", EntryPoint:="ReleaseCapture")>
    Private Shared Sub ReleaseCapture()
    End Sub
    <DllImport("user32.DLL", EntryPoint:="SendMessage")>
    Private Shared Sub SendMessage(ByVal hWnd As System.IntPtr, ByVal wMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer)
    End Sub

    Private Sub PanelBarraTitulo_MouseMove(sender As Object, e As MouseEventArgs) Handles PanelBarraTitulo.MouseMove
        ReleaseCapture()
        SendMessage(Me.Handle, &H112&, &HF012&, 0)
    End Sub

    Private Sub btnCerrar_Click(sender As Object, e As EventArgs) Handles btnCerrar.Click
        Application.Exit()

    End Sub


    Dim lx, ly As Integer
    Dim sw, sh As Integer

    Private Sub btnMinimizar_Click(sender As Object, e As EventArgs) Handles btnMinimizar.Click
        Me.WindowState = FormWindowState.Minimized
    End Sub

    Private Sub btnMaximizar_Click(sender As Object, e As EventArgs) Handles btnMaximizar.Click
        lx = Me.Location.X
        ly = Me.Location.Y
        sw = Me.Size.Width
        sh = Me.Size.Height
        btnMaximizar.Visible = False
        btnRestaurar.Visible = True
        Me.Size = Screen.PrimaryScreen.WorkingArea.Size
        Me.Location = Screen.PrimaryScreen.WorkingArea.Location

    End Sub
    Private Sub btnRestaurar_Click(sender As Object, e As EventArgs) Handles btnRestaurar.Click
        Me.Size = New Size(sw, sh)
        Me.Location = New Point(lx, ly)
        btnMaximizar.Visible = True
        btnRestaurar.Visible = False
    End Sub

#End Region


    'abrir formularios' 

    Private Sub abrirFormulario(Of Miform As {Form, New})()
        Dim Formulario As Form
        Formulario = PanelFormularios.Controls.OfType(Of Miform)().FirstOrDefault()
        If Formulario Is Nothing Then
            Formulario = New Miform()
            Formulario.TopLevel = False

            Formulario.FormBorderStyle = FormBorderStyle.None
            Formulario.Dock = DockStyle.Fill



            PanelFormularios.Controls.Add(Formulario)
            PanelFormularios.Tag = Formulario

            AddHandler Formulario.FormClosed, AddressOf Me.CerrarFormulario
            Formulario.BringToFront()
            Formulario.Show()
        Else
            Formulario.BringToFront()
        End If


    End Sub



    'cerrar Formulario con  Excepciones
    Private Sub Cerrarforms()
        Dim OpenForms As Form() = Application.OpenForms.Cast(Of Form)().ToArray()
        For Each thisForm As Form In OpenForms
            If thisForm.Name <> "Form1" AndAlso thisForm.Name <> "Form2" Then thisForm.Close()
        Next
    End Sub





    'cerrar todos los formularios Exceto el que esta abierto
    Private Sub closeForm()
        My.Application.OpenForms.Cast(Of Form)() _
              .Except({Me}) _
              .ToList() _
              .ForEach(Sub(form) form.Close())
    End Sub

    'cerrar un formulario al abrir otro'
    Private Sub OpenFormPanel(Of Miform As {Form, New})()
        closeForm()
        Dim Formulario As Form
        Formulario = PanelFormularios.Controls.OfType(Of Miform)().FirstOrDefault()
        If Formulario Is Nothing Then
            Formulario = New Miform()
            Formulario.TopLevel = False
            'Formulario.FormBorderStyle = FormBorderStyle.None
            'Formulario.Dock = DockStyle.Fill
            PanelFormularios.Controls.Add(Formulario)
            PanelFormularios.Tag = Formulario
            AddHandler Formulario.FormClosed, AddressOf Me.CerrarFormulario
            Formulario.BringToFront()
            Formulario.Show()
        Else
            Formulario.BringToFront()
        End If
    End Sub


    'Metodo para  Cerrar FOrms'

    Private Sub CerrarFormulario(ByVal sender As Object, ByVal e As FormClosedEventArgs)

        ' condicion para ver si esta abierto el Form

        If (Application.OpenForms("Form1") Is Nothing) Then
            Button1.BackColor = Color.FromArgb(0, 86, 98)
        End If
        If (Application.OpenForms("Form2") Is Nothing) Then
            Button2.BackColor = Color.FromArgb(0, 86, 98)

        End If
        If (Application.OpenForms("Form3") Is Nothing) Then
            Button3.BackColor = Color.FromArgb(0, 86, 98)
        End If

    End Sub


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        abrirFormulario(Of Form1)()
        Button1.BackColor = Color.FromArgb(36, 146, 155)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        abrirFormulario(Of Form2)()

        Button2.BackColor = Color.FromArgb(36, 146, 155)
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        abrirFormulario(Of Form3)()

        Button3.BackColor = Color.FromArgb(36, 146, 155)
    End Sub

End Class
