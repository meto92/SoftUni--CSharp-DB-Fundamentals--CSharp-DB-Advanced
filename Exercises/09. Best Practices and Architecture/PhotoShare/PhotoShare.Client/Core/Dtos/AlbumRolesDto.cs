using System.Collections.Generic;

namespace PhotoShare.Client.Core.Dtos
{
    public class AlbumRolesDto
    {
        public AlbumDto Album { get; set; }

        public ICollection<AlbumRoleDto> AlbumRoles { get; set; }
    }
}