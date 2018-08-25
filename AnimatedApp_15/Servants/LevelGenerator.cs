using AnimatedApp_15.Components;
using System;
using System.IO;

namespace AnimatedApp_15.Servants
{
    class LevelGenerator
    {
        Random random = new Random();
        Level level;
        int p1;
        int p2;
        int p3;
        int p4;
        int p5;
        int p6;
        int high;
        int length;
        char[,] levelMap;
        public LevelGenerator(Level level)
        {
            this.level = level;
        }
        /// <summary>
        /// Генерация уровня заданной длины на основе пользовательских параметров:
        /// </summary>
        /// <param name="length">Длина уровня</param>
        /// <param name="p1">Через сколько этажей строится новый мост</param>
        /// <param name="p2">Максимальная высота моста на уровне</param>
        /// <param name="p3">Макс. длина моста от центра уровня</param>
        /// <param name="p4">Макс. длина моста, коэффициент 2</param>
        /// <param name="p5">Процентная часть кристаллов на уровне</param>
        /// <param name="p6">Процентная часть противников на уровне</param>
        /// <param name="kBack">Отношение длины фона к длине игрового поля</param>
        /// <returns></returns>
        private string[] Generation(int length, int p1, int p2, int p3, int p4, int p5, int p6, float kBack)
        {
            high = 18;
            this.length = length;
            levelMap = new char[length, high];
            this.p1 = p1;
            this.p2 = p2;
            this.p3 = p3;
            this.p4 = p4;
            this.p5 = p5;
            this.p6 = p6;

            Start:
            for (int i = 0; i < length; i++) // Initializing array with default chars
            {
                for (int j = 0; j < high - 1; j++) levelMap[i, j] = '0';
                levelMap[i, high - 1] = 'Y';
            }
            string[] levelGeneration = new string[high];

            /// Scheme of random level generation algorithm
            /// Create a brick bridges and platforms
            BricksGeneration(); // firstly
            BricksCorrection(); // check bricks
            if (PortalsGeneration() < 2)
                goto Start;
            /// Create portals
            /// Create gems
            GemsGeneration();
            EnemiesGeneration();
            /// And enemies for current level

            for (int j = 0; j < high; j++)
                for (int i = 0; i < length; i++)
                    levelGeneration[j] += levelMap[i, j];

            level.background.kBack = kBack;
            return File.ReadAllLines(Environment.CurrentDirectory + "/Content/Levels/map1_1.lvl");
        }
        private int PortalsGeneration()
        {
            int portals = 0;
            for (int i = 1; i < length; i++)
            {
                for (int j = 1; j < high - 1; j++)
                {
                    for (int k = i - 1; k < i + 2; k++)
                    {
                        if (levelMap[k, j - 1] != '0'
                            || levelMap[k, j] != '0'
                            || !(levelMap[k, j + 1] == 'X' || levelMap[k, j + 1] == 'Y'))
                            goto Watch;
                    }
                    levelMap[i, j] = 'P';
                    portals++;
                    goto Enter;
                Watch: { }
                }
            }
            Enter: { }

            for (int i = length - 2; i > 0; i--)
            {
                for (int j = 1; j < high - 1; j++)
                {
                    for (int k = i - 1; k < i + 2; k++)
                    {
                        if (levelMap[k, j - 1] != '0'
                            || levelMap[k, j] != '0'
                            || !(levelMap[k, j + 1] == 'X' || levelMap[k, j + 1] == 'Y'))
                            goto Watch;
                    }
                    levelMap[i, j] = 'Q';
                    portals++;
                    goto _Quit;
                Watch: { }
                }
            }
            _Quit: { }
            return portals;
        } // Enter and Exit from the level
        public string[] Generate()
        {
            switch (level.currentLevel)
            {
                default:
                    return Generation(32, 3, 3, 10, 16, 4, 14, 1.35f);
            }
        } // Constructor
        private void GemsGeneration()
        {
            switch (level.currentLevel)
            {
                /// Level #01 generation:
                #region
                default:
                    for (int i = 0; i < length; i++)
                    {
                        for (int j = 0; j < high - 3; j++)
                        {
                            if (levelMap[i, j + 1] == '0' && levelMap[i, j + 2] == '0' && levelMap[i, j + 3] == 'X')
                            {
                                if (random.Next(p5) == 0)
                                    levelMap[i, j + 2] = 'G';
                            }
                        }
                    }
                    break;
                #endregion
            }
        }
        private void BricksGeneration()
        {
            switch (level.currentLevel)
            {
                /// Level #01 generation:
                #region
                default:
                    for (int j = high - 3; j != p2; j -= p1)
                    {
                        int count = 0;
                        bool block;
                        if (random.Next(2) == 0) block = false;
                        else block = true;

                        while (count < length)
                        {
                            int shift = random.Next(Math.Min((Math.Abs(length - count * 2)) / 2 + p3, p4));
                            if (!block)
                                while (--shift > 0 && count < length)
                                    levelMap[count++, j] = 'X';
                            else
                                while (--shift > 0 && count < length)
                                    count++;
                            block = !block;
                        }
                    }
                    break;
                #endregion
            }
        }
        private void BricksCorrection()
        {
            for (int j = p2; j < high - 5; j += p1)
            {
                for (int i = 1; i < length; i++)
                    if (levelMap[i - 1, j] == 'X' && levelMap[i, j] == '0'
                        && !BlockFixRight(i, j))
                            levelMap[i, j] = 'X';

                for (int i = length - 1; i > 0; i--)
                    if (levelMap[i - 1, j] == '0' && levelMap[i, j] == 'X'
                        && !BlockFixLeft(i, j))
                            levelMap[i - 1, j] = 'X';

                for (int i = 0; i < length; i++)
                    if (levelMap[i, j] == '0')
                        goto Watch;
                levelMap[random.Next(length), j] = '0';
            Watch: { }
            }
        }
        private bool BlockFixLeft(int i, int j)
        {
            for (int k = i; k > i - 4; k--)
                if (k > -1 && levelMap[k, j + p1] == 'X' && levelMap[k, j] == '0')
                    return true;

            for (int k = i - 1; k < i - 5; k--)
                if (k < length && levelMap[k, j] == 'X' && j != p2)
                {
                    for (int l = i - 1; l > k; l--)
                        if (levelMap[l, j - p1] == '0'
                            && levelMap[l - 1, j - p1] == 'X')
                                goto Watch;
                    return true;
                }
            Watch:

            for (int l = j - p1; l != 3; l -= p1)
            {
                if (levelMap[i, l] == 'X' && levelMap[i - 1, l] == '0')
                {
                    for (int m = l + p1; m < j; m += p1)
                        if (levelMap[i - 1, m] == 'X')
                            return false;
                    return true;
                }

                for (int k = i - 1; k < i - 6; k--)
                {
                    if (k > 0 && levelMap[k, l] == '0' && levelMap[k - 1, l] == 'X')
                    {
                        for (int m = i + 1; m < k; m++)
                            for (int n = l + p1; n < j; n += p1)
                                if (levelMap[m, n] == 'X') return false;
                        return true;
                    }
                }
            }
            return false;
        }
        private bool BlockFixRight(int i, int j)
        {
            for (int k = i; k < i + 4; k++)
                if (k < length && levelMap[k, j + p1] == 'X' && levelMap[k, j] == '0')
                    return true;

            for (int k = i + 1; k < i + 5; k++)
                if (k < length && levelMap[k, j] == 'X' && j != p2)
                {
                    for (int l = i + 1; l < k; l++)
                        if (levelMap[l, j - p1] == '0'
                            && levelMap[l + 1, j - p1] == 'X')
                                goto Watch;
                    return true;
                }
            Watch:

            for (int l = j - p1; l != 3; l -= p1)
            {
                if (levelMap[i - 1, l] == 'X' && levelMap[i, l] == '0')
                {
                    for (int m = l + p1; m < j; m += p1)
                        if (levelMap[i, m] == 'X')
                            return false;
                    return true;
                }

                for (int k = i + 1; k < i + 6; k++)
                {
                    if (k < length - 1 && levelMap[k, l] == '0' && levelMap[k + 1, l] == 'X')
                    {
                        for (int m = i; m < k; m++)
                            for (int n = l + p1; n < j; n += p1)
                                if (levelMap[m, n] == 'X') return false;
                        return true;
                    }
                }
            }
            
            int p = i;
            while (p-- > 1)
            {
                if (levelMap[p, j] == '0' && levelMap[p + 1, j] == 'X')
                    if (BlockFixLeft(p, j))
                        return true;
                    else break;
            }
            return false;
        }
        private void EnemiesGeneration()
        {
            switch (level.currentLevel)
            {
                /// Level #01 generation:
                #region
                default:
                    for (int i = 0; i < length - 1; i++)
                    {
                        for (int j = 0; j < high - 3; j++)
                        {
                            if (random.Next(p6) == 0)
                            {
                                if (levelMap[i, j + 1] == '0' && levelMap[i, j + 2] == '0' && levelMap[i, j + 3] == 'X')
                                {
                                    int count = 0;
                                    for (int k = i - 1; k > -1; k--)
                                    {
                                        if (levelMap[k, j + 3] == 'X')
                                        {
                                            count++;
                                            if (levelMap[k, j + 2] == '★') goto Watch;
                                        }
                                        else break;
                                    }
                                    for (int k = i + 1; k < length; k++)
                                    {
                                        if (levelMap[k, j + 3] == 'X')
                                        {
                                            count++;
                                            if (levelMap[k, j + 2] == '★') goto Watch;
                                        }
                                        else break;
                                    }
                                    if (count < 2) goto Watch;
                                    levelMap[i, j + 2] = '★';
                                Watch: { }
                                }
                            }
                        }
                    }
                    break;
                #endregion
            }
        } // Artificial Intellect Bots
    }
}