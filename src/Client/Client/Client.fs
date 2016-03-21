namespace Monitex

type Client(endpointUrl : string, apiKey : string, apiSecret : string, ?data : string, ?ttl : int) = 
    member this.X = "F#"
