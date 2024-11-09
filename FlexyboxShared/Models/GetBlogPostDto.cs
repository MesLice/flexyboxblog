using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexyboxShared.Models
{
    internal class GetBlogPostDto
    {
        public required string Title { get; set; }
        public required string Content { get; set; }
    }
}
