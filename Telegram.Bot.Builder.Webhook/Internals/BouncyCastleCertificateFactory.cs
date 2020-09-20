using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Operators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using Telegram.Bot.Builder.Webhook.Extensions;

namespace Telegram.Bot.Builder.Webhook.Internals
{
    internal sealed class BouncyCastleCertificateFactory : ICertificateFactory, IDisposable
    {
        private readonly ILogger _logger;
        private readonly SelfSignedCertificateOptions _certOptions;
        private readonly WebhookOptions _webhookOptions;

        private readonly object _locker;
        private bool _isInitialized;
        private Org.BouncyCastle.X509.X509Certificate _publicCertificate;
        private System.Security.Cryptography.X509Certificates.X509Certificate2 _privateCertificate;
        private byte[] _publicCertificateBytes;

        public BouncyCastleCertificateFactory
            (
                ILogger<BouncyCastleCertificateFactory> logger,
                IOptions<SelfSignedCertificateOptions> selfSignedCertificateOptions,
                IOptions<WebhookOptions> webhookOptions
            )
        {
            _logger = logger;
            _certOptions = selfSignedCertificateOptions.Value;
            _webhookOptions = webhookOptions.Value;

            _locker = new object();
            _isInitialized = false;
            _publicCertificate = null!;
            _privateCertificate = null!;
            _publicCertificateBytes = null!;
        }

        public Stream GetPublicCertificate()
        {
            Initialize();
            return new MemoryStream((byte[])_publicCertificateBytes.Clone(), false);
        }

        public System.Security.Cryptography.X509Certificates.X509Certificate2 GetPrivateCertificate()
        {
            Initialize();
            return _privateCertificate;
        }

        public void Dispose()
        {
            _publicCertificate = null!;
            _publicCertificateBytes = null!;
            _privateCertificate?.Dispose();
            _privateCertificate = null!;
        }

        private void Initialize()
        {
            if (_isInitialized)
                return;

            lock (_locker)
            {
                if (_isInitialized)
                    return;

                _logger.LogWarning("Generating new self-signed certificate.");
                var stopwatch = Stopwatch.StartNew();
                try
                {
                    GenerateCertificate();
                }
                catch(Exception exception)
                {
                    exception = new Exception("Error on generating self-signed certificate.", exception);
                    _logger.LogCritical(exception, exception.Message);
                    throw exception;
                }
                stopwatch.Stop();

                _logger.LogWarning("Certificate generated in \"{ElapsedTime}\": " +
                                   $"{Environment.NewLine}{{PublicCertificate}}{Environment.NewLine}" +
                                   $"{{PublicCertificateBytes}}{Environment.NewLine}",
                    stopwatch.Elapsed, _publicCertificate.ToString(), Encoding.UTF8.GetString(_publicCertificateBytes));

                Thread.MemoryBarrier();
                _isInitialized = true;
            }
        }

        private void GenerateCertificate()
        {
            var random = new SecureRandom();

            var rsaGenerator = new RsaKeyPairGenerator();
            rsaGenerator.Init(new KeyGenerationParameters(random, _certOptions.RsaBitSize));

            var keyPair = rsaGenerator.GenerateKeyPair();

            var notBefore = DateTime.UtcNow.Date;
            var notAfter = notBefore.Add(_certOptions.ValidityTime);
            var issuer = GetIssuer();

            var x509Gen = new X509V3CertificateGenerator();
            x509Gen.SetPublicKey(keyPair.Public);
            x509Gen.SetSubjectDN(issuer);
            x509Gen.SetIssuerDN(issuer);
            x509Gen.SetNotBefore(notBefore);
            x509Gen.SetNotAfter(notAfter);
            x509Gen.SetSerialNumber(BigInteger.ProbablePrime(16 * 8, random));

            var x509 = x509Gen.Generate(new Asn1SignatureFactory("SHA512withRSA", keyPair.Private, random));
            _publicCertificate = x509;
            _publicCertificateBytes = x509.ToPemBytes();

            _privateCertificate = new System.Security.Cryptography.X509Certificates.X509Certificate2(DotNetUtilities.ToX509Certificate(x509))
                .CopyWithPrivateKey(DotNetUtilities.ToRSA((RsaPrivateCrtKeyParameters)keyPair.Private));
        }

        private X509Name GetIssuer()
        {
            var builder = new StringBuilder();

            if (_certOptions.CountryName != null)
            {
                builder.Append(X509Name.C.Is(_certOptions.CountryName));
            }
            if (_certOptions.StateName != null)
            {
                builder.AppendIfAny(",").Append(X509Name.ST.Is(_certOptions.StateName));
            }
            if (_certOptions.Locality != null)
            {
                builder.AppendIfAny(",").Append(X509Name.L.Is(_certOptions.Locality));
            }
            if (_certOptions.OrganizationUnitName != null)
            {
                builder.AppendIfAny(",").Append(X509Name.O.Is(_certOptions.OrganizationUnitName));
            }
            if (_certOptions.EmailAddress != null)
            {
                builder.AppendIfAny(",").Append(X509Name.EmailAddress.Is(_certOptions.EmailAddress));
            }
            builder.AppendIfAny(",").Append(X509Name.CN.Is(_certOptions.CommonName ?? _webhookOptions.GetCommonName()));

            return new X509Name(builder.ToString());
        }
    }
}
