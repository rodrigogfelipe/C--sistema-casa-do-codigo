using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace CasaDoCodigo.Models
{
    [DataContract]
    public class BaseModel
    {
        [DataMember]
        public int Id { get; protected set; } //O modificador protected deixará visível o atributo para todas as outras classes e subclasses que pertencem ao mesmo pacote.
    }
}
