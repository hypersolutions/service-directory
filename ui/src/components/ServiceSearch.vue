<template>
  <v-container>
    <v-row>
      <v-col cols="12">
        <v-text>
          <p class="text-h5 pa-5" testid="description">
            Use the search tools below to find services within a certain distance of the entered postcode. These will be ordered from the closest to furthest.
          </p>
        </v-text>
      </v-col>
    </v-row>
    <v-row>
      <v-col cols="4">
        <PostcodeSearchForm :searching="searching" @onSearch="doPostcodeSearch"/>
      </v-col>
      <v-col cols="8" v-if="searching">
        <ProgressBar message="Searching for services"/>
      </v-col>
      <v-col cols="8" v-else>
        <v-list lines="one" class="overflow-y-auto" max-height="800" v-if="services.length > 0">
          <v-list-item v-for="item in services" :key="item.id">
            <SearchResult :item="item"/>
          </v-list-item>
        </v-list>
        <v-text v-else>
          <p class="text-h4 text-center pa-5">No services found</p>
        </v-text>
      </v-col>
    </v-row>
  </v-container>

</template>

<script setup>

const services = defineModel('services', {default: []});
const searching = defineModel('searching', {default: false});
const maxResults = [5, 10];

async function doPostcodeSearch(args) {
  try {
    searching.value = true;
    services.value = [];

    const url = `/api/service/search/?postcode=${args.postcode}&distance=${args.distance}&maxResults=${args.maxResults}`;
    const options = {method: 'GET'};

    const response = await fetch(url, options);

    if (response.ok) {
      services.value = await response.json();
    }
  }
  catch (error) {
    console.error(error);
  }
  finally {
    // Allow the progress to run for a second or two
    setTimeout(() => (searching.value = false), 2000);
  }
}
</script>

<style scoped>

</style>
