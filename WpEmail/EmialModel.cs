namespace WpfEmail
{
    using System;
    using System.Collections.Generic;

    public class EmialModel
    {
        private Random _random = new Random();

        private string _mainBreed= "";
        private List<string> _otherBreeds = new List<string>();

        public event EventHandler OnMainBreedChaged;
        public event EventHandler OnOtherBreedsChanged;

        const int MAX_ORD = 28;

        private char OrdToChar(int ord)
        {
            char nextChar = ' ';

            while (ord < 0) { ord += MAX_ORD; }
            ord = ord % MAX_ORD;

            if (ord < 26)
            {
                nextChar = (char)('a' + ord);
            }
            else if (ord < 27)
            {
                nextChar = '@';
            }
            else if (ord < 28)
            {
                nextChar = '.';
            }
            return nextChar;
        }
        private int CharToOrd(char chr)
        {
            switch (chr) {
                case '@': return 26;
                case '.': return 27;
                default: return (chr - 'a');
            }
        }

        public void ComplicateMainBreed()
        {
            _mainBreed += OrdToChar(_random.Next(MAX_ORD));
            OnMainBreedChaged.Invoke(this, null);
        }
        public int GetBreedCount()
        {
            return _otherBreeds.Count;
        }
        public bool CanSaveMainBreed()
        {
            return _mainBreed.Length > 0;
        }
        public bool CanEvolveMainBreed()
        {
            return _mainBreed.Length > 0;
        }
        public string GetMainBreedName()
        {
            return _mainBreed;
        }
        public void SaveMainBreed()
        {
            _otherBreeds.Add(_mainBreed);
            _mainBreed = "";
            OnMainBreedChaged.Invoke(this, null);
            OnOtherBreedsChanged.Invoke(this, null);
        }
        public string GetBreedName(int i)
        {
            return _otherBreeds[i];
        }

        public void EvolveMainBreed()
        {
            _mainBreed = Evolve(_mainBreed);
            OnMainBreedChaged.Invoke(this, null);
        }
        public void EvolveBreed(int i)
        {
            _otherBreeds[i] = Evolve(_otherBreeds[i]);
            OnOtherBreedsChanged.Invoke(this, null);
        }
        private string Evolve(string breed)
        {
            var place = _random.Next(breed.Length);
            var change = _random.Next(2) > 0 ? -1 : 1;

            char[] charArr = breed.ToCharArray();
            charArr[place] = OrdToChar(CharToOrd(breed[place]) + change);
            return new string(charArr);
        }
        public void CloneBreed(int i)
        {
            _mainBreed += _otherBreeds[i];
            OnMainBreedChaged.Invoke(this, null);
        }
        public void KillBreed(int i)
        {
            _otherBreeds.RemoveAt(i);
            OnOtherBreedsChanged.Invoke(this, null);
        }
    }
}