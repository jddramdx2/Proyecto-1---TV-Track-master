﻿using System;
using System.Linq;
using System.Windows.Forms;
using Proyecto__1___TV_Track.Views;
using Proyecto_1_TV_Track.Data;
using Proyecto_1_TV_Track.Models;

namespace Proyecto_1_TV_Track.Views
{
    public partial class LoginForm : Form
    {
        private UserRepository userRepo; // Maneja los usuarios del archivo CSV

        public LoginForm()
        {
            InitializeComponent();
            userRepo = new UserRepository();
            LoadRoles(); // Carga los roles disponibles

            // Conecta los botones con sus respectivas funciones
            btnLogin.Click += btnLogin_Click;
            btnRegister.Click += btnRegister_Click;
        }

        /// <summary>
        /// Agrega los roles disponibles al combo box.
        /// </summary>
        private void LoadRoles()
        {
            cmbRole.Items.Clear();
            cmbRole.Items.Add("Admin");
            cmbRole.Items.Add("User");
            cmbRole.SelectedIndex = 0; // Para que siempre tenga un valor y no quede vacío
        }

        /// <summary>
        /// Verifica si el usuario existe y permite el inicio de sesión.
        /// </summary>
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string role = cmbRole.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(role))
            {
                MessageBox.Show("Ingrese su nombre y seleccione un rol, por favor.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var users = userRepo.GetUsers(); // Busca usuarios en el CSV
                var user = users.FirstOrDefault(u =>
                    string.Equals(u.Name, username, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(u.Role, role, StringComparison.OrdinalIgnoreCase));

                if (user != null) // Verifica el usuario en la lista y confirma
                {
                    MessageBox.Show($"Bienvenido, {user.Name}!", "INGRESO EXITOSO :)", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Hide(); // Oculta la ventana de login
                    if (user.Role == "Admin")
                    {
                        // 📌 Open AdminChoiceForm so the Admin can choose
                        AdminChoiceForm choiceForm = new AdminChoiceForm();

                        if (choiceForm.ShowDialog() == DialogResult.OK)
                        {
                            if (choiceForm.SelectedOption == AdminChoiceForm.AdminOption.MovieCatalog)
                            {
                                MovieForm movieForm = new MovieForm();
                                movieForm.ShowDialog();
                            }
                            else if (choiceForm.SelectedOption == AdminChoiceForm.AdminOption.Reports)
                            {
                                AdminReportForm adminForm = new AdminReportForm();
                                adminForm.ShowDialog();
                            }
                        }
                    }
                    else
                    {
                        // 📌 Si el usuario es normal, solo abre la lista de películas
                        MovieForm movieForm = new MovieForm();
                        movieForm.ShowDialog();
                    }

                    // 📌 Una vez que el usuario cierra su ventana, mostramos el login otra vez
                    this.Show();
                }
                else
                {
                    MessageBox.Show("Oh oh, ¿existes?", "Error de ingreso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hubo un problema al iniciar sesión: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Abre la ventana para registrar un nuevo usuario.
        /// </summary>
        private void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                UserRegisterForm registerForm = new UserRegisterForm(); // Se abre la pantalla de registro
                registerForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hubo un problema al abrir el formulario de registro: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Este método es para cuando se hace clic en la etiqueta, pero no hace nada.
        /// </summary>
        private void label1_Click(object sender, EventArgs e)
        {
            // Solo está aquí para evitar errores del sistema
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
        }
    }
}