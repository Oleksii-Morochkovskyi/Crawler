/* eslint-disable */
import type {Location} from "@/components/Location" 

export class LinkData {
  public id: number
  public url: string;
  public baseUrl: string;
  public responseTime: number;
  public Location: Location; 
  constructor(id: number, url: string, baseUrl: string, responseTime: number, location: Location) {
    this.id = id;
    this.url = url;
    this.baseUrl = baseUrl;
    this.responseTime = responseTime;
    this.Location = location
  }
}