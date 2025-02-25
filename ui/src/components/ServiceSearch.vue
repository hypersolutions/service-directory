<template>
  <v-container>
    <v-row>
      <v-col cols="4">
        <PostcodeSearchForm :searching="searching" @onSearch="doPostcodeSearch"/>
      </v-col>
      <v-col cols="8" v-if="searching">
        <ProgressBar message="Searching for services"/>
      </v-col>
      <v-col cols="8" v-else>
        <v-list lines="one" class="overflow-y-auto" max-height="800">
          <v-list-item v-for="item in services" :key="item.id">
            <SearchResult :item="item"/>
          </v-list-item>
        </v-list>
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
