// ***********************************************************************
// Assembly         : ACBr.Net.Consulta
// Author           : RFTD
// Created          : 02-20-2017
//
// Last Modified By : RFTD
// Last Modified On : 02-20-2017
// ***********************************************************************
// <copyright file="ACBrConsultaSintegra.cs" company="ACBr.Net">
//		        		   The MIT License (MIT)
//	     		    Copyright (c) 2014 - 2017 Grupo ACBr.Net
//
//	 Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following conditions:
//	 The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//	 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using ACBr.Net.Core;
using ACBr.Net.Core.Exceptions;
using ACBr.Net.Core.Extensions;

namespace ACBr.Net.Consulta.Sintegra
{
    /// <summary>
    /// Class ACBrConsultaSintegra. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="ACBrComponentConsulta" />
    [ToolboxBitmap(typeof(ACBrConsultaSintegra), "ACBr.Net.Consulta.ACBrConsultaSintegra.bmp")]
    public sealed class ACBrConsultaSintegra : ACBrComponent
    {
        #region Events

        public event EventHandler<CaptchaEventArgs> OnGetCaptcha;

        #endregion Events

        #region Methods

        /// <summary>
        /// Gets the captcha.
        /// </summary>
        /// <param name="uf"></param>
        /// <returns>Image.</returns>
        public Image GetCaptcha(ConsultaUF uf)
        {
            var consultaSintegra = GetConsulta(uf);
            return consultaSintegra.GetCaptcha();
        }

        /// <summary>
        /// Consultas the specified CNPJ.
        /// </summary>
        /// <param name="uf"></param>
        /// <param name="cnpj">The CNPJ.</param>
        /// <param name="ie">The ie.</param>
        /// <param name="captcha">The captcha.</param>
        /// <returns>ACBrEmpresa.</returns>
        public ACBrEmpresa Consulta(ConsultaUF uf, string cnpj = "", string ie = "", string captcha = "")
        {
            Guard.Against<ACBrException>(ie.IsEmpty() && cnpj.IsEmpty(), "� necess�rio digitar o CNPJ ou a Inscri��o Estadual.");

            var consultaSintegra = GetConsulta(uf);

            if (captcha.IsEmpty() && OnGetCaptcha != null)
            {
                var e = new CaptchaEventArgs();
                OnGetCaptcha.Raise(this, e);

                captcha = e.Captcha;
            }

            return consultaSintegra.Consulta(cnpj, ie, captcha);
        }

        #region Private Methods

        private static IConsultaSintegra GetConsulta(ConsultaUF uf)
        {
            switch (uf)
            {
                case ConsultaUF.SP: return ConsultaSintegraSP.Instance;
                case ConsultaUF.MS: return ConsultaSintegraMS.Instance;
                default: throw new NotImplementedException("Consulta para este estado n�o implementado.");
            }
        }

        #endregion Private Methods

        #region Protected Methods

        protected override void OnInitialize()
        {
        }

        protected override void OnDisposing()
        {
        }

        #endregion Protected Methods

        #endregion Methods
    }
}