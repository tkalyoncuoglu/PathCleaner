using System;
using System.Collections.Generic;
using System.Linq;

namespace PathCleaner
{
    class Path
    {
        public List<string> Folders { get; private set; }

        public Path(List<string> folders)
        {
            Folders = folders;
        }
    }


    
}