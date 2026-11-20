using Microsoft.Extensions.DependencyInjection;
using ShopTARpe25.ApplicationServices.Services;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ShopTARpe25.RealEstateTest.Macros;

namespace ShopTARpe25.RealEstateTest
{
    //Klass on abstraktne, mis tähendab, et sellest otse objekti luua ei saa
    public abstract class TestBase
    {
        //Iga testi käivitamisel loob konstruktor uue teenuse konteineri (ServiceCollection).
        protected IServiceProvider ServiceProvider { get; set; }

        protected TestBase()
        {
            //Loob uue teenuse kogu vajalikuks teenuste testimiseks.
            var services = new ServiceCollection();
            //Kustub välja teenuste seadistamise meetodi ning ehitab valmis ServiceProvider'i.
            SetupServices(services);
            ServiceProvider = services.BuildServiceProvider();
        }

        public virtual void SetupServices(IServiceCollection services)
        {
            //Seob liidese konkreetse teostusega, et testid saaksid kinnisvarateenuseid kasutada.
            services.AddScoped<IRealEstateServices, RealEstateServices>();

            //Päris andmebaasi asemel luuakse operatiivmällu ajutine andmebaas nimega "TEST".
            //Sellega muudab testid kiireks ega riku tegelikke andmeid.
            services.AddDbContext<ShopTARpe25Context>(x =>
            {
                x.UseInMemoryDatabase("TEST");
                //Keelab hoiatused, mis tekkivad siis, kui kood üritab mälupõhises andmebaasis
                //kasutada transaktsioone.
                x.ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });

            //Kustub välja Macrode klassi
            RegisterMacros(services);
        }

        //Vabastab ressursid peale kasutamist
        public void Dispose()
        {

        }

        //Lühike abimeetod, millega testiklass saab mugavalt küsida konteinerist
        //soovitud teenust või objekti.
        protected T Svc<T>()
        {
            return ServiceProvider.GetService<T>();
        }

        //Otsib reflektsiooni (reflection) abil projektist üles kõik klassid,
        //mis realiseerivad liidest IMacros, ning registreerib need teenustena.
        //Neid kasutatakse tavaliselt testandmete genereerimiseks.
        private void RegisterMacros(IServiceCollection services)
        {
            var macroBaseType = typeof(IMacros);

            //loab kokku kõik tüübid sellest assemblyst, kus Macro on määratletud
            var macros = macroBaseType.Assembly.GetTypes()
                //valib tüübid, mis rakendavad IMacros liidest, aga ei ole ise liidesed
                //ega abstraktsed klassid
                .Where(t => macroBaseType.IsAssignableFrom(t)
                && !t.IsInterface && !t.IsAbstract);

            //käib leitud makrode classid ükshaaval läbi
            foreach (var macro in macros)
            {
                //kasutab ühte macro eksemplaari.
                services.AddSingleton(macro);
            }
        }

    }
}
