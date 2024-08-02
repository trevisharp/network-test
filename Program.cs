
ApplicationConfiguration.Initialize();

var form = new Form
{
    FormBorderStyle = FormBorderStyle.None,
    WindowState = FormWindowState.Maximized
};

form.KeyDown += (o, e) =>
{
    if (e.KeyCode == Keys.Escape)
        Application.Exit();
};

Application.Run(form);