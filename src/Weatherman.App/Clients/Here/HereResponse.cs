using System.Collections.Generic;

namespace Weatherman.App.Clients.Here
{
    internal class HereResponse
    {
        public HereResponseModel Response { get; set; }

        internal sealed class HereResponseModel
        {
            public IEnumerable<HereResponseModelView> View { get; set; }

            internal sealed class HereResponseModelView
            {
                public IEnumerable<HereResponseViewResult> Result { get; set; }

                internal sealed class HereResponseViewResult
                {
                    public HereResponseViewResultLocation Location { get; set; }

                    internal sealed class HereResponseViewResultLocation
                    {
                        public HereResponseViewResultLocationDisplayPosition DisplayPosition { get; set; }
                        public HereResponseViewResultLocationDisplayAddress Address { get; set; }

                        internal sealed class HereResponseViewResultLocationDisplayPosition
                        {
                            public float Latitude { get; set; }
                            public float Longitude { get; set; }
                        }

                        internal sealed class HereResponseViewResultLocationDisplayAddress
                        {
                            public string Label { get; set; }
                            public string Country { get; set; }
                            public string State { get; set; }
                            public string City { get; set; }
                            public string District { get; set; }
                            public IEnumerable<KeyValuePair<string, string>> AdditionalData { get; set; }
                        }
                    }
                }
            }
        }
    }
}
