/* eslint-disable */
import type {LinkData} from "@/components/LinkData" 

export class LinkPerformance {
  public urls: LinkData[] | null;
  public urlsFromHtml: [] | null;
  public urlsFromXml: [] | null;

  constructor(urls: LinkData[], urlsFromHtml: [], urlsFromXml: []) {
    this.urls = urls;
    this.urlsFromHtml = urlsFromHtml;
    this.urlsFromXml = urlsFromXml;
  }
}