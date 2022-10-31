using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Game
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public double speed = 0;
        public double xCor = 300.0;

        private double x_user = 0;
        private double y_user = 0;

        private int score = 0;

        private double timer_spawn_stone = 0.8;
        private double speed_stone = 5;

        public user_ship user = new user_ship();
        //таймер передвижения пользователя
        private DispatcherTimer timer = new DispatcherTimer();
        //Таймер появление астеройдов астеройдов
        private DispatcherTimer timer_stone = new DispatcherTimer();
        //Таймер появление астеройдов астеройдов
        private DispatcherTimer timer_stone_move = new DispatcherTimer();
        //Массив остеройдов
        List<Block> stone = new List<Block>();


        public MainWindow()
        {
            InitializeComponent();

            //Запуск таймера пользователя
            timer.Tick += new EventHandler(update);
            timer.Interval = TimeSpan.FromSeconds(0.01);


            //Запуск таймера астеройдов
            timer_stone.Tick += new EventHandler(update_stone);
            timer_stone.Interval = TimeSpan.FromSeconds(0.5);

            //Запуск таймера астеройдов
            timer_stone_move.Tick += new EventHandler(update_stone_move);
            timer_stone_move.Interval = TimeSpan.FromSeconds(0.01);

        }
        public double score_timer = 0.0;

        public void update(Object obj, EventArgs e)
        {
            score_timer += 0.01;
            Text.Content = Math.Round(score_timer,2);

            y_user = (double)user.GetValue(TopProperty);
            x_user = (double)user.GetValue(LeftProperty);

            if (xCor > 500.0 - 40.0) {
                xCor = 500.0-39.0;
            }
            else if (xCor < 0.0)
            {
                xCor = 1.0;
            }
            xCor += speed;
            art_ship(xCor);

        }

        public void update_stone_move(Object obj, EventArgs e)
        {
            for (int i = 0; i <= stone.Count() - 2; i++)
            {
                backgroundCanvas.Children.Remove(stone[i]);
                double y = (double)stone[i].GetValue(TopProperty) + speed_stone;
                double x = (double)stone[i].GetValue(LeftProperty);
                if (y < 800.0)
                {
                    stone[i].SetValue(TopProperty, y );
                    backgroundCanvas.Children.Add(stone[i]);
                }

                if (((y + 70 > y_user && y + 70 < y_user + 20) ||(y > y_user && y < y_user + 20)) &&((x_user>x && x_user < x+70) || (x_user + 20 > x && x_user + 20 < x + 70)))
                {
                    timer_stone_move.Stop();             
                }
            }

        }
        public void update_stone(Object obj, EventArgs e)
        {
            stone.Add( new Block());

            Random r = new Random();
            
            stone[stone.Count()-1].SetValue(LeftProperty, (double)(r.Next(0, 450)));
            stone[stone.Count()-1].SetValue(TopProperty, -100.0);

            backgroundCanvas.Children.Add(stone[stone.Count()-1]);

            score++;

            if (score % 15 == 0)
            {
                speed_stone += 1.5;
                if (timer_spawn_stone > 0.05)
                {
                    timer_spawn_stone -= 0.09;
                }
            }

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.D  )
            {
                speed = 10;
            }
            else if (e.Key == Key.A )
            {
                speed = -10;
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.D)
            {
                speed = 0;
            }
            else if (e.Key == Key.A)
            {
                speed = 0;
            }
        }
        //Отрисовка корабля пользователя
        private void art_ship(double xCor)
        {
            backgroundCanvas.Children.Remove(user);
            user.SetValue(LeftProperty, xCor);
            user.SetValue(TopProperty, 500.0);

            backgroundCanvas.Children.Add(user);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Все переменные приравниваем к начальным значениям
            score_timer = 0.0;
            speed = 0;
            xCor = 300.0;

            x_user = 0;
            y_user = 0;

            score = 0;

            timer_spawn_stone = 0.8;
            speed_stone = 5;
            //Удаление всех элементов с поля
            backgroundCanvas.Children.Clear();

            //Рисуем корабль
            art_ship(xCor);
            //Clear List
            stone.Clear();
            //Запуск таймера пользователя
            timer.Start();

            //Запуск таймера астеройдов
            timer_stone.Start();
            //Запуск таймера астеройдов
            timer_stone_move.Start();
        }
    }
}
