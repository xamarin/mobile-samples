using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamarinFormsScreenShot
{
    public interface IScreen
    {
        Task<byte[]> CaptureScreenAsync();
    }
}
